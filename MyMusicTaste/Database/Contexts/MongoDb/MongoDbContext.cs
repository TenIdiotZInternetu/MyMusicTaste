using MongoDB.Driver;

namespace MyMusicTaste.Database.Contexts.MongoDb;

/// <summary>
/// Provides a MongoDB client and a method to establish a connection.
/// </summary>
public class MongoDbContext
{
    private static readonly ServerApiVersion API_VERSION = ServerApiVersion.V1;
    
    /// <summary>
    /// The singleton MongoDB client instance.
    /// </summary>
    public static MongoClient? Client { get; private set; }
    
    /// <summary>
    /// Connects to the MongoDB database using the provided connection string.
    /// </summary>
    /// <param name="key">The MongoDB connection string.</param>
    /// <exception cref="ArgumentException">Thrown if the connection string is null or empty.</exception>
    public static void Connect(string? key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("A connection string is required to connect to the database.");
        }

        var mongoSettings = MongoClientSettings.FromConnectionString(key);
        mongoSettings.ServerApi = new ServerApi(API_VERSION);
        
        Client = new MongoClient(mongoSettings);
    }
}