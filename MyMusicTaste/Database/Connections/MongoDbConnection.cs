using MongoDB.Driver;

namespace MyMusicTaste.Database;

public class MongoDbConnection
{
    private static readonly ServerApiVersion ApiVersion = ServerApiVersion.V1;
    public static MongoClient Client { get; private set; }
    
    public static void Connect(string? key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("A connection string is required to connect to the database.");
        }

        var mongoSettings = MongoClientSettings.FromConnectionString(key);
        mongoSettings.ServerApi = new ServerApi(ApiVersion);
        
        Client = new MongoClient(mongoSettings);
    }
}