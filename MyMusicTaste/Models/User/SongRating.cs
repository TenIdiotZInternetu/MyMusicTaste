using MongoDB.Bson;

namespace MyMusicTaste.Models;

/// <summary>
/// Represents a rating given by a user to a song.
/// </summary>
public class SongRating : Model
{
    /// <summary>
    /// Sentinel value of the Rating field, indicating that a song has not been rated.
    /// </summary>
    public const byte NOT_RATED = 255;
    
    public ObjectId UserId { get; set; }
    public ObjectId SongId { get; set; }
    public byte Rating { get; set; } = NOT_RATED;
    
    public bool IsRated => Rating != NOT_RATED;
}