using MongoDB.Driver;
using MyMusicTaste.CanonicalModels;

namespace MyMusicTaste.Database.Contexts.MongoDb.Models;

public class MongoSongRatingModel : SongRating
{
    public MongoDBRef? UserRef { get; set; }
    public MongoDBRef? SongRef { get; set; }
}