using MongoDB.Driver;

namespace MyMusicTaste.Database.Contexts.MongoDb;

public class MongoDbContext
{
    private static readonly ServerApiVersion API_VERSION = ServerApiVersion.V1;
    public static MongoClient? Client { get; private set; }
    
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