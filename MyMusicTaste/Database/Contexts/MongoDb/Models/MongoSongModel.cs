using MongoDB.Driver;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Models;

public class MongoSongModel : Song
{
    public List<MongoDBRef> RatingsRef { get; set; } = new();
}