using MongoDB.Bson;
using MongoDB.Driver;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

public class MongoSongRatingListing : ISongRatingListing
{
    private IMongoCollection<SongRating> _collection = MongoCollectionFactory.Create<SongRating>();

    private IDbRepository<User> _usersRepo;
    private IDbRepository<Song> _songsRepo;

    public MongoSongRatingListing(IDbRepository<User> usersRepo, IDbRepository<Song> songsRepo)
    {
        _usersRepo = usersRepo;
        _songsRepo = songsRepo;
    }

    public Task<SongRating> GetSongRatingAsync(string songId, string userId)
    {
        var filter = Builders<SongRating>.Filter
            .Where(rating => rating.SongId.ToString() == songId &&
                             rating.UserId.ToString() == userId);
        
        return _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<SongRating>> GetRatingsByUserAsync(string userId)
    {
        var filter = Builders<SongRating>.Filter
            .Eq(rating => rating.UserId, new ObjectId(userId));
        
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<SongRating>> GetRatingsBySongAsync(string songId)
    {
        var filter = Builders<SongRating>.Filter
            .Eq(rating => rating.SongId, new ObjectId(songId));
        
        return await _collection.Find(filter).ToListAsync();
    }
}