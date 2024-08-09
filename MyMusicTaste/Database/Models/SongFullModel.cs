using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MyMusicTaste.Database.Connections;

namespace MyMusicTaste.Database.Models;

public class SongFullModel : ISongModel, IMongoDbModel<SongFullModel>
{
    private static readonly DateOnly InvalidDate = DateOnly.MaxValue;

    public static IMongoCollection<SongFullModel> Collection { get; } = MongoDbContext.Client
        .GetDatabase("Core")
        .GetCollection<SongFullModel>("Songs");
    
    [BsonElement("_id")]
    public ObjectId Id { get; set; }

    public string? Title { get; set; }
    public string? Author { get; set; }

    public string Album { get; set; } = "Unknown";

    public string Genre { get; set; } = "Unknown";

    public DateOnly ReleaseDate { get; set; } = InvalidDate;
    
    public string ReleaseDateString => ReleaseDate == InvalidDate ? 
        "Unknown" : ReleaseDate.ToString();
    
    public SongFullModel ToFullModel() => this;
}