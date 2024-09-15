using MongoDB.Driver;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

public class MongoSongSearch : ISearchOperation<Song>
{
    private const string SEARCH_INDEX = "SongsIndex";
    private IMongoCollection<Song> _collection = MongoCollectionFactory.Create<Song>();

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