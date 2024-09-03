using MongoDB.Driver;
using MyMusicTaste.Database.Connections;
using MyMusicTaste.Database.Contexts.MongoDb.Models;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb;

public class MongoCollectionFactory
{
    private record struct CollectionInfo(string DbName, string CollectionName);

    private static readonly Dictionary<Type, CollectionInfo> _collectionMapping = new()
    {
        { typeof(User), new CollectionInfo("Core", "Users") },
        { typeof(Song), new CollectionInfo("Core", "Songs") },
        { typeof(MongoSongModel), new CollectionInfo("Core", "Songs") }
    };
    
    public static IMongoCollection<TModel> Create<TModel>() where TModel : Model
    {
        var client = MongoDbContext.Client;
        var collInfo = _collectionMapping[typeof(TModel)];
        return client.GetDatabase(collInfo.DbName).GetCollection<TModel>(collInfo.CollectionName);
    }
}