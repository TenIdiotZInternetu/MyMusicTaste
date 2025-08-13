using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Operations;

public interface ICommentsListing
{
    public Task<IEnumerable<Comment>> GetCommentsByUserAsync(string userId, int resultCount);
    public Task<IEnumerable<Comment>> GetCommentsByPageAsync(CommentPageType pageType, string pageId, int resultCount);
}