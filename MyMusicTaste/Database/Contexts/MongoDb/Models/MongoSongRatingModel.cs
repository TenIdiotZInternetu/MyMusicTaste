using MongoDB.Bson;
using MongoDB.Driver;
using MyMusicTaste.CanonicalModels;

namespace MyMusicTaste.Database.Contexts.MongoDb.Models;

public class MongoSongRatingModel : SongRating
{
    public ObjectId? UserId { get; set; }
    public ObjectId? SongId { get; set; }

    public bool IsValid => UserId != null && SongId != null;
}