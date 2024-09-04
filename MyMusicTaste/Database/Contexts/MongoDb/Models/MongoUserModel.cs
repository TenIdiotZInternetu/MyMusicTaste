using MongoDB.Driver;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Models;

public class MongoUserModel : User
{
    public List<MongoDBRef> SongRatingsRefs { get; set; } = new();
    public MongoUserModel(string username) : base(username)
    {
        Username = username;
    }
}