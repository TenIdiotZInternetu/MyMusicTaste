using MongoDB.Bson;

namespace MyMusicTaste.Models;

public class SongRating : Model
{
    public const byte NOT_RATED = 255;
    
    public ObjectId UserId { get; set; }
    public ObjectId SongId { get; set; }
    public byte Rating { get; set; } = NOT_RATED;
}