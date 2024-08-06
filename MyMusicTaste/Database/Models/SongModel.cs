using System.ComponentModel.DataAnnotations;
using MongoDB.Driver;

namespace MyMusicTaste.Database.Models;

public class SongModel
{
    private static readonly DateOnly InvalidDate = DateOnly.MaxValue;

    public static IMongoCollection<SongModel> Collection = MongoDbConnection.Client
        .GetDatabase("Core")
        .GetCollection<SongModel>("Songs");
    
    [Required]
    [StringLength(256)]
    public string Title { get; set; }
    
    [Required]
    [StringLength(256)]
    public string Artist { get; set; }

    public string Album { get; set; } = "Single";

    public string Genre { get; set; } = "Unknown";

    public DateOnly ReleaseDate { get; set; } = InvalidDate;
    
    public string ReleaseDateString => ReleaseDate == InvalidDate ? 
        "Unknown" : ReleaseDate.ToString();
}