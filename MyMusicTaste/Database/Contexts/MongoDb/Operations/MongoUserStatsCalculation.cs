using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

/// <summary>
/// Calculates statistics for a user based on their song ratings in MongoDB.
/// </summary>
public class MongoUserStatsCalculation : IUserStatsCalculation
{
    [BsonIgnoreExtraElements]
    private class _SongRatingJoin : SongRating {
        public required Song Song { get; set; }
    }
    
    [BsonIgnoreExtraElements] private record struct _MeanResult(string Name, double Mean);
    
    private const string ALBUMS_FACET = "Album";
    private const string AUTHORS_FACET = "Author";
    private const string GENRES_FACET = "Genre";
    
    private readonly IMongoCollection<SongRating> _ratingsCollection = MongoCollectionFactory.Create<SongRating>();
    private readonly IMongoCollection<Song> _songsCollection = MongoCollectionFactory.Create<Song>();
    
    /// <summary>
    /// Calculates statistics for a user based on their ratings, including average ratings by album, author, and genre.
    /// </summary>
    /// <param name="userId">The ID of the user to calculate statistics for.</param>
    /// <returns>A task for the user's statistics.</returns>
    public async Task<UserStats> CalculateUserStatsAsync(string userId)
    {
        var filterBuilder = Builders<SongRating>.Filter;
        var filter = filterBuilder.Eq(rating => rating.UserId, new ObjectId(userId)) &
                     filterBuilder.Ne(rating => rating.Rating, SongRating.NOT_RATED);

        var aggregation = await _ratingsCollection.Aggregate()
            .Match(filter)
            .Lookup<SongRating, Song, _SongRatingJoin>(
                foreignCollection: _songsCollection,
                localField: rating => rating.SongId,
                foreignField: song => song.Id,
                @as: join => join.Song
            )
            .Unwind<_SongRatingJoin, _SongRatingJoin>(join => join.Song)
            .Facet(
                MeanFacet(ALBUMS_FACET, join => join.Song.Album),
                MeanFacet(AUTHORS_FACET, join => join.Song.Author),
                MeanFacet(GENRES_FACET, join => join.Song.Genre)
            )
            .FirstOrDefaultAsync();

        return new UserStats
        {
            AlbumsByMean = FacetResult(aggregation, ALBUMS_FACET),
            AuthorsByMean = FacetResult(aggregation, AUTHORS_FACET),
            GenresByMean = FacetResult(aggregation, GENRES_FACET)
        };
    }
    
    /// <summary>
    /// Creates a facet pipeline that computes the mean rating for a specific grouping (e.g., album, author, genre).
    /// </summary>
    /// <param name="facetName">The name of the facet.</param>
    /// <param name="groupKey">The key to group by.</param>
    /// <returns>The aggregation facet.</returns>
    private AggregateFacet<_SongRatingJoin, _MeanResult> MeanFacet(
        string facetName,
        Expression<Func<_SongRatingJoin, string?>> groupKey)
    {
        var nullFilter = Builders<_SongRatingJoin>.Filter.Ne(groupKey, null);
        
        return AggregateFacet.Create(
            name: facetName,
            pipeline: new EmptyPipelineDefinition<_SongRatingJoin>()
                .Match(nullFilter)
                .Group(
                    id: groupKey,
                    group: g => new _MeanResult(
                        g.Key!.ToString(),
                        g.Average(entry => (double)entry.Rating)
                    )
                )
        );
    }

    /// <summary>
    /// Extracts and orders the results from a facet aggregation.
    /// </summary>
    /// <param name="aggregation">The aggregation result containing facets.</param>
    /// <param name="facetName">The name of the facet to extract.</param>
    /// <returns>A list of tuples with the name and mean rating.</returns>
    private List<(string, double)> FacetResult(AggregateFacetResults aggregation, string facetName)
    {
        var facet = aggregation.Facets.First(f => f.Name == facetName);
        return facet.Output<_MeanResult>()
            .OrderByDescending(res => res.Mean)
            .Select(res => (res.Name, res.Mean))
            .ToList();
    }
}