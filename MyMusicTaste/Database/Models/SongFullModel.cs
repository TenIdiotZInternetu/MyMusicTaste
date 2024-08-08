using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace MyMusicTaste.Database.Models;

public class SongFullModel : ISongModel
{
    private static readonly DateOnly InvalidDate = DateOnly.MaxValue;

    public static IMongoCollection<SongFullModel> Collection = MongoDbConnection.Client
        .GetDatabase("Core")
        .GetCollection<SongFullModel>("Songs");
    
    [BsonElement("_id")]
    public ObjectId Id { get; set; }

    public string? Title { get; set; }
    public string? Artist { get; set; }

    public string Album { get; set; } = "Unknown";

    public string Genre { get; set; } = "Unknown";

    public DateOnly ReleaseDate { get; set; }
    
    public string ReleaseDateString => ReleaseDate == InvalidDate ? 
        "Unknown" : ReleaseDate.ToString();
    
    public SongFullModel ToFullModel() => this;
}