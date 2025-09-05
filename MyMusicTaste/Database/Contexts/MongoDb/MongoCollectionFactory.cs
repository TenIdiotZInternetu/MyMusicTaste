using MongoDB.Driver;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb;

/// <summary>
/// Provides a factory for creating MongoDB collections for different model types.
/// </summary>
public static class MongoCollectionFactory
{
    private record struct CollectionInfo(string DbName, string CollectionName);

    private static readonly Dictionary<Type, CollectionInfo> COLL_MAPPING = new()
    {
        { typeof(User), new CollectionInfo("Core", "Users") },
        { typeof(Song), new CollectionInfo("Core", "Songs") },
        { typeof(SongRating), new CollectionInfo("Core", "SongRatings") },
        { typeof(Comment), new CollectionInfo("Core", "Comments") }
    };
    
    /// <summary>
    /// Creates a MongoDB collection for the specified model type.
    /// </summary>
    /// <typeparam name="TModel">The model type of the collection. Must inherit from <see cref="Model"/>.</typeparam>
    /// <returns>An <see cref="IMongoCollection{TModel}"/> for the specified model type.</returns>
    /// <exception cref="NullReferenceException">Thrown if the Mongo client has not been initialized.</exception>
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