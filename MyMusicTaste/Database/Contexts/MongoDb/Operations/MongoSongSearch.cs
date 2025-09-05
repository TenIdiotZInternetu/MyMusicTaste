using MongoDB.Driver;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

/// <summary>
/// Performs search operations for users in MongoDB.
/// </summary>
public class MongoSongSearch : ISearchOperation<Song>
{
    private const string SEARCH_INDEX = "SongsIndex";
    private IMongoCollection<Song> _collection = MongoCollectionFactory.Create<Song>();

    /// <summary>
    /// Searches users by username using autocomplete.
    /// </summary>
    /// <param name="query">The search query string.</param>
    /// <param name="resultsCount">The maximum number of results to return.</param>
    /// <returns>A task for the collection of matching users, or null if the query is empty.</returns>
    public async Task<IEnumerable<Song>?> SearchAsync(string query, int resultsCount)
    {
        if (string.IsNullOrEmpty(query))
        {
            return null;
        }
        
        return await _collection.Aggregate()
            .Search(
                Builders<Song>.Search.Autocomplete(song => song.Title, query),
                indexName: SEARCH_INDEX)
            .Limit(resultsCount)
            .ToListAsync();
    }
}