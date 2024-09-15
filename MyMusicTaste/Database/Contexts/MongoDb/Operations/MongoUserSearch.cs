using MongoDB.Driver;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

public class MongoUserSearch : ISearchOperation<User>
{
    private const string SEARCH_INDEX = "UsersIndex";
    private IMongoCollection<User> _collection = MongoCollectionFactory.Create<User>();

    public async Task<IEnumerable<User>?> SearchAsync(string query, int resultsCount)
    {
        if (string.IsNullOrEmpty(query))
        {
            return null;
        }

        return await _collection.Aggregate()
            .Search(
                Builders<User>.Search.Autocomplete(user => user.Username, query),
                indexName: SEARCH_INDEX)
            .Limit(resultsCount)
            .ToListAsync();
    }
}