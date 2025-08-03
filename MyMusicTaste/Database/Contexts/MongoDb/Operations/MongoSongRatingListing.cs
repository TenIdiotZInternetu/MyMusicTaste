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

    public async Task<IEnumerable<SongRating>> GetRatingsByUserAsync(User user)
    {
        var filter = Builders<SongRating>.Filter
            .Eq(rating => rating.UserId, user.Id);
        
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<SongRating>> GetRatingsBySongAsync(Song song)
    {
        var filter = Builders<SongRating>.Filter
            .Eq(rating => rating.SongId, song.Id);
        
        return await _collection.Find(filter).ToListAsync();
    }
}