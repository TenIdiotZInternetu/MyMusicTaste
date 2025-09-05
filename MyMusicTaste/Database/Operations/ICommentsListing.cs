using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Operations;

/// <summary>
/// Provides methods for retrieving comments from the database.
/// </summary>
public interface ICommentsListing
{
    /// <summary>
    /// Retrieves comments made by a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose comments to retrieve.</param>
    /// <param name="resultCount">The maximum number of comments to return.</param>
    /// <returns>A task for the collection of comments.</returns>
    public Task<IEnumerable<Comment>> GetCommentsByUserAsync(string userId, int resultCount);
    
    /// <summary>
    /// Retrieves comments associated with a specific page, ordered by creation time descending.
    /// </summary>
    /// <param name="pageType">The type of the page the comments belong to.</param>
    /// <param name="pageId">The ID of the page.</param>
    /// <param name="resultCount">The maximum number of comments to return.</param>
    /// <returns>A task for the list of comments.</returns>
    public Task<IEnumerable<Comment>> GetCommentsByPageAsync(CommentPageType pageType, string pageId, int resultCount);
}