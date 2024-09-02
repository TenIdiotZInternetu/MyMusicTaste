using MongoDB.Driver;
using MyMusicTaste.Database.Connections;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

public class SongSearch : ISearchOperation<Song>
{
    private IMongoCollection<Song> _collection = MongoCollectionFactory.Create<Song>();
    
    public Task<IEnumerable<Song>> Search(string query, int resultsCount)
    {
        throw new NotImplementedException();
    }
}