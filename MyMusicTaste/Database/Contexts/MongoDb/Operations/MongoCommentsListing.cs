using MongoDB.Bson;
using MongoDB.Driver;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

public class MongoCommentsListing : ICommentsListing
{
    private IMongoCollection<Comment> _collection = MongoCollectionFactory.Create<Comment>();
    
    public async Task<IEnumerable<Comment>> GetCommentsByUserAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Comment>> GetCommentsByPageAsync(CommentPageType pageType, string pageId)
    {
        var filter = Builders<Comment>.Filter.Eq(comment => comment.CommentPageType, pageType) &
                     Builders<Comment>.Filter.Eq(comment => comment.PageId, new ObjectId(pageId));
        
        return await _collection.Find(filter).ToListAsync();
    }
}