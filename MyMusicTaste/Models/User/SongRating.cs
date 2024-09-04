using MongoDB.Bson;

namespace MyMusicTaste.Models;

public class SongRating : Model
{
    private const byte NOT_RATED = 255;
    
    public User? User { get; set; }
    public Song? Song { get; set; }
    public byte Rating { get; set; } = NOT_RATED;

    public bool IsValid => User != null && Song != null;
}