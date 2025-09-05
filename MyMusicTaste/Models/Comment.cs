using MongoDB.Bson;

namespace MyMusicTaste.Models;

/// <summary>
/// Types of pages that a comment can be associated with.
/// </summary>
public enum CommentPageType { Song, User }

/// <summary>
/// Represents a comment posted by a user on a certain page.
/// </summary>
public class Comment : Model
{
    private static readonly DateTime INVALID_DATE = DateTime.MaxValue;
    
    /// <summary>
    /// The ID of the user who made the comment.
    /// </summary>
    public required ObjectId UserId { get; set; }
    
    /// <summary>
    /// The type of page the comment was posted in.
    /// </summary>
    public required CommentPageType CommentPageType { get; set; }
    
    /// <summary>
    /// The ID of the page the comment was posted to.
    /// </summary>
    public required ObjectId PageId { get; set; }
    
    
    public string Content { get; set; } = string.Empty;
    public DateTime CreationTime { get; set; } = INVALID_DATE;
    public DateTime LastEditTime { get; set; } = INVALID_DATE;
    
    public bool CreationTimeValid => CreationTime != INVALID_DATE;
    public bool LastEditTimeValid => LastEditTime != INVALID_DATE;
}