using MongoDB.Bson;
using MongoDB.Driver;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

/// <summary>
/// Provides methods for retrieving comments from the MongoDb database.
/// </summary>
public class MongoCommentsListing : ICommentsListing
{
    private IMongoCollection<Comment> _collection = MongoCollectionFactory.Create<Comment>();
    
    /// <summary>
    /// Retrieves comments made by a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose comments to retrieve.</param>
    /// <param name="resultCount">The maximum number of comments to return.</param>
    /// <returns>A task for the list of comments.</returns>
    public Task<IEnumerable<Comment>> GetCommentsByUserAsync(string userId, int resultCount)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Retrieves comments associated with a specific page, ordered by creation time descending.
    /// </summary>
    /// <param name="pageType">The type of the page the comments belong to.</param>
    /// <param name="pageId">The ID of the page.</param>
    /// <param name="resultCount">The maximum number of comments to return.</param>
    /// <returns>A task for the collection of comments.</returns>
    public async Task<IEnumerable<Comment>> GetCommentsByPageAsync(CommentPageType pageType, string pageId, int resultCount)
    {
        var filter = Builders<Comment>.Filter
             .Where(comment => comment.CommentPageType == pageType && 
                               comment.PageId == new ObjectId(pageId));
        
        var dateSort = Builders<Comment>.Sort.Descending(c => c.CreationTime);

        return await _collection.Aggregate()
            .Match(filter)
            .Sort(dateSort)
            .Limit(resultCount)
            .ToListAsync();
    }
}