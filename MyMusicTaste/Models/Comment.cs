using MongoDB.Bson;

namespace MyMusicTaste.Models;

public enum CommentPageType { Song, User }

public class Comment : Model
{
    public required ObjectId UserId { get; set; }
    public required CommentPageType CommentPageType { get; set; }
    public required ObjectId PageId { get; set; }
    public required string Content { get; set; }
    public required DateTime DateAndTime { get; set; }
}