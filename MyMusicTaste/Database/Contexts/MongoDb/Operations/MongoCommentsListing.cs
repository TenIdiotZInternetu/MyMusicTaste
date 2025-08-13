using MongoDB.Bson;
using MongoDB.Driver;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

public class MongoCommentsListing : ICommentsListing
{
    private IMongoCollection<Comment> _collection = MongoCollectionFactory.Create<Comment>();
    
    public async Task<IEnumerable<Comment>> GetCommentsByUserAsync(string userId, int resultCount)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Comment>> GetCommentsByPageAsync(CommentPageType pageType, string pageId, int resultCount)
    {
        var filter = Builders<Comment>.Filter
             .Where(comment => comment.CommentPageType == pageType && 
                               comment.PageId == new ObjectId(pageId));
        
        var dateSort = Builders<Comment>.Sort.Descending(c => c.DateAndTime);

        return await _collection.Aggregate()
            .Match(filter)
            .Sort(dateSort)
            .Limit(resultCount)
            .ToListAsync();
    }
}