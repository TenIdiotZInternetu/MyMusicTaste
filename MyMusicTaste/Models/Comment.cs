using MongoDB.Bson;

namespace MyMusicTaste.Models;

public enum CommentPageType { Song, User }

public class Comment : Model
{
    private static readonly DateTime INVALID_DATE = DateTime.MaxValue;
    
    public required ObjectId UserId { get; set; }
    public required CommentPageType CommentPageType { get; set; }
    public required ObjectId PageId { get; set; }
    public required string Content { get; set; }
    public required DateTime CreationTime { get; set; }
    public DateTime LastEditTime { get; set; } = INVALID_DATE;
    
    public bool LastEditTimeValid => LastEditTime != INVALID_DATE;
}