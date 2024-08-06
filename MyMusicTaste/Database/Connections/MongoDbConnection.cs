using MongoDB.Driver;

namespace MyMusicTaste.Database;

public class MongoDbConnection
{
    public static MongoClient Client { get; private set; }
    
    public static bool Connect(string? key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("A connection string is required to connect to the database.");
        }
        
        Client = new MongoClient(key);
        return true;
    }
}