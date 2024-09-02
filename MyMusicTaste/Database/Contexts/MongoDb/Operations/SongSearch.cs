using MongoDB.Bson;
using MongoDB.Driver;
using MyMusicTaste.Database.Connections;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

public class SongSearch : ISearchOperation<Song>
{
    private IMongoCollection<Song> _collection = MongoCollectionFactory.Create<Song>();

    public List<Song> Search(string query, int resultsCount)
    {
        return _collection.Aggregate()
            .Search(
                Builders<Song>.Search.Autocomplete(song => song.Title, query),
                indexName: "SongsIndex")
            .Limit(resultsCount)
            .ToList();
    }
}