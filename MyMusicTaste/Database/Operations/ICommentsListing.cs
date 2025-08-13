using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Operations;

public interface ICommentsListing
{
    public Task<IEnumerable<Comment>> GetCommentsByUserAsync(string userId);
    public Task<IEnumerable<Comment>> GetCommentsByPageAsync(CommentPageType pageType, string pageId);
}