using MongoDB.Driver;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

public class MongoSongSearch : ISearchOperation<Song>
{
    private IMongoCollection<Song> _collection = MongoCollectionFactory.Create<Song>();

    public List<Song>? Search(string query, int resultsCount)
    {
        if (string.IsNullOrEmpty(query))
        {
            return null;
        }
        
        return _collection.Aggregate()
            .Search(
                Builders<Song>.Search.Autocomplete(song => song.Title, query),
                indexName: "SongsIndex")
            .Limit(resultsCount)
            .ToList();
    }
}