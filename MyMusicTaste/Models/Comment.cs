using MongoDB.Bson;

namespace MyMusicTaste.Models;

public enum CommentPageType { Song, User }

public class Comment : Model
{
    private static readonly DateTime INVALID_DATE = DateTime.MaxValue;
    
    public required ObjectId UserId { get; set; }
    public required CommentPageType CommentPageType { get; set; }
    public required ObjectId PageId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreationTime { get; set; } = INVALID_DATE;
    public DateTime LastEditTime { get; set; } = INVALID_DATE;
    
    public bool CreationTimeValid => CreationTime != INVALID_DATE;
    public bool LastEditTimeValid => LastEditTime != INVALID_DATE;
}