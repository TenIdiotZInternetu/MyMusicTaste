using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

public class MongoSongsStatsCalculation : ISongStatsCalculation
{
    [BsonIgnoreExtraElements] private record _MeanResult(double Mean);
    [BsonIgnoreExtraElements] private record _MedianResult(double Median);
    [BsonIgnoreExtraElements] private record _DistributionBucket(int LowerBound, int Count);
    
    private const string MEAN = "Mean";
    private const string MEDIAN = "Median";
    private const string COUNT = "Count";
    private const string DISTRIBUTION = "Distribution";
    
    private readonly IMongoCollection<SongRating> _ratingsCollection = MongoCollectionFactory.Create<SongRating>();
    
    public async Task<SongStats> CalculateSongStatsAsync(Song song)
    {
        var filterBuilder = Builders<SongRating>.Filter;
        var filter = filterBuilder.Eq(rating => rating.SongId, song.Id) &
                     filterBuilder.Ne(rating => rating.Rating, SongRating.NOT_RATED);


        var aggregation = await _ratingsCollection.Aggregate()
            .Match(filter)
            .Facet(
                CountFacet(),
                MeanFacet(),
                MedianFacet(),
                DistributionFacet()
            )
            .FirstOrDefaultAsync();


        int totalListens = (int)(aggregation.Facets[0].Output<AggregateCountResult>().FirstOrDefault()?.Count ?? 0);
        if (totalListens <= 0)
        {
            return new SongStats(); // Contains no data
        }

        var meanDoc = aggregation.Facets[1].Output<_MeanResult>().FirstOrDefault();
        float meanRating = (float)(meanDoc?.Mean ?? 0);

        var medianDoc = aggregation.Facets[2].Output<_MedianResult>().FirstOrDefault();
        float medianRating = (float)(medianDoc?.Median ?? 0);

        var buckets = aggregation.Facets[3].Output<_DistributionBucket>();
        
        return new SongStats
        {
            AverageRating = meanRating,
            MedianRating = medianRating,
            TotalListens = totalListens,
            RatingDistribution = CreateDistribution(buckets)
        };
    }

    private AggregateFacet<SongRating, _MeanResult> MeanFacet()
    {
        return AggregateFacet.Create(
            name: MEAN,
            pipeline: new EmptyPipelineDefinition<SongRating>()
                .Group(
                    id: entry => BsonNull.Value,
                    group: g => new _MeanResult(g.Average(entry => entry.Rating)))
        );
    }

    private AggregateFacet<SongRating, _MedianResult> MedianFacet()
    {
        return AggregateFacet.Create(
            name: MEDIAN,
            pipeline: new EmptyPipelineDefinition<SongRating>()
                .AppendStage(
                    stage: new BsonDocument {
                        {"$group", new BsonDocument{
                            {"_id", BsonNull.Value},
                            {"Median", new BsonDocument("$median", new BsonDocument {
                                {"input", $"$Rating"},
                                {"method", "approximate"}
                            })},
                        }}},
                    outputSerializer: BsonSerializer.SerializerRegistry.GetSerializer<_MedianResult>())
        );
    }

    private AggregateFacet<SongRating, AggregateCountResult> CountFacet()
    {
        return AggregateFacet.Create(
            name: COUNT,
            pipeline: new EmptyPipelineDefinition<SongRating>()
                .Count()
        );
    }

    private AggregateFacet<SongRating, _DistributionBucket> DistributionFacet()
    {
        return AggregateFacet.Create(
            name: DISTRIBUTION,
            pipeline: new EmptyPipelineDefinition<SongRating>()
                .Bucket(
                    groupBy: entry => entry.Rating,
                    boundaries: SongStats.CreateDistributionBoundaries(),
                    output: bucket => new _DistributionBucket(bucket.Key, bucket.Count()))
        );
    }
    
    private int[] CreateDistribution(IReadOnlyList<_DistributionBucket> buckets)
    {
        var boundaries = SongStats.CreateDistributionBoundaries();
        int[] distribution = new int[boundaries.Length - 1]; 
        
        for (int i = 0; i < distribution.Length; i++)
        {
            int boundary = boundaries[i];
            var match = buckets.FirstOrDefault(bucket => bucket.LowerBound == boundary);
            distribution[i] = match?.Count ?? 0;
        }
        
        return distribution;
    }
}