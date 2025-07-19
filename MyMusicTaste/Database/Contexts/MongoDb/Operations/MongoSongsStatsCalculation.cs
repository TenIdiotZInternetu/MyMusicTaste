using MongoDB.Bson;
using MongoDB.Driver;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

public class MongoSongsStatsCalculation : ISongStatsCalculation
{
    private readonly IMongoCollection<SongRating> _ratingsCollection = MongoCollectionFactory.Create<SongRating>();
    
    public async Task<SongStats> CalculateSongStats(Song song)
    {
        var filter = Builders<SongRating>.Filter
            .Eq(rating => rating.SongId, song.Id);
        
        var pipeline = await _ratingsCollection.Aggregate()
            .Match(filter)
            .SetPipeline(avgResult, avgPipeline) // Computes Average and Median
            .SetPipeline(distroResult, distroPipeline) // Divides into buckets
            .SetPipeline(countResult, countPipeline) // you get the deal
            
            
            .AppendStage<BsonDocument>(new BsonDocument{
                {"$group", new BsonDocument{
                    {"_id", BsonNull.Value},
                    {"Average", new BsonDocument("$avg", "$Rating")},
                    {"Median", new BsonDocument("$median", new BsonDocument {
                        {"input", $"$Rating"},
                        {"method", "approximate"}
                    })},
                }}})
            .Bucket(
                groupBy: doc => doc.
            )
    }

    private AggregateFacet<SongRating, float> MeanFacet()
    {
        return AggregateFacet.Create(
            name: "Average",
            pipeline: new EmptyPipelineDefinition<SongRating>()
                .Group(
                    id: entry => BsonNull.Value,
                    group: g => (float) g.Average(rating => rating.Rating))
        );
    }

    private AggregateFacet<SongRating, BsonDocument> MedianFacet()
    {
        return AggregateFacet.Create(
            name: "Median",
            pipeline: new EmptyPipelineDefinition<SongRating>()
                .AppendStage<SongRating, SongRating, BsonDocument>(
                    new BsonDocument {
                        {"$group", new BsonDocument{
                            {"_id", BsonNull.Value},
                            {"Median", new BsonDocument("$median", new BsonDocument {
                                {"input", $"$Rating"},
                                {"method", "approximate"}
                            })},
                        }}})
        );
    }

    private AggregateFacet<SongRating, AggregateCountResult> CountFacet()
    {
        return AggregateFacet.Create(
            name: "Count",
            pipeline: new EmptyPipelineDefinition<SongRating>()
                .Count()
        );
    }

    private AggregateFacet<SongRating, AggregateBucketResult<int>> DistributionFacet()
    {
        return AggregateFacet.Create(
            name: "Distribution",
            pipeline: new EmptyPipelineDefinition<SongRating>()
                .Bucket(
                    groupBy: entry => entry.Rating,
                    boundaries: SongStats.CreateDistributionBoundaries())
        );
    }
}