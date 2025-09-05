using MongoDB.Driver;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

/// <summary>
/// Performs search operations for songs in MongoDB.
/// </summary>
public class MongoUserSearch : ISearchOperation<User>
{
    private const string SEARCH_INDEX = "UsersIndex";
    private IMongoCollection<User> _collection = MongoCollectionFactory.Create<User>();

    /// <summary>
    /// Searches songs by title using autocomplete.
    /// </summary>
    /// <param name="query">The search query string.</param>
    /// <param name="resultsCount">The maximum number of results to return.</param>
    /// <returns>A task for the collection of matching songs, or null if the query is empty.</returns>
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