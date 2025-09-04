using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

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

    private List<(string, double)> FacetResult(AggregateFacetResults aggregation, string facetName)
    {
        var facet = aggregation.Facets.First(f => f.Name == facetName);
        return facet.Output<_MeanResult>()
            .OrderByDescending(res => res.Mean)
            .Select(res => (res.Name, res.Mean))
            .ToList();
    }
}