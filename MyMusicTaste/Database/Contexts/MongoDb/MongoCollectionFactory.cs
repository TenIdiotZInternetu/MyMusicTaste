using MongoDB.Driver;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb;

public static class MongoCollectionFactory
{
    private record struct CollectionInfo(string DbName, string CollectionName);

    private static readonly Dictionary<Type, CollectionInfo> COLL_MAPPING = new()
    {
        { typeof(User), new CollectionInfo("Core", "Users") },
        { typeof(Song), new CollectionInfo("Core", "Songs") },
        { typeof(SongRating), new CollectionInfo("Core", "SongRatings") }
    };
    
    public static IMongoCollection<TModel> Create<TModel>() where TModel : Model
    {
        var client = MongoDbContext.Client;

        if (client == null)
        {
            throw new NullReferenceException("Mongo Client not initialized");
        }
        
        var collInfo = COLL_MAPPING[typeof(TModel)];
        return client.GetDatabase(collInfo.DbName).GetCollection<TModel>(collInfo.CollectionName);
    }
}