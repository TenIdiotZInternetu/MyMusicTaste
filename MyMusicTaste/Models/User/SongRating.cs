using MongoDB.Bson;

namespace MyMusicTaste.Models;

public class SongRating : Model
{
    public ObjectId UserId { get; set; }
    public ObjectId SongId { get; set; }
    public int Rating { get; set; }
}