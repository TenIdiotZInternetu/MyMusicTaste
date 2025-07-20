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

    public async Task<IEnumerable<SongRating>> GetRatingsByUserAsync(User user)
    {
        var filter = CreateUserFilter(user);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<SongRating>> GetRatingsBySongAsync(Song song)
    {
        var filter = CreateSongFilter(song);
        return await _collection.Find(filter).ToListAsync();
    }

    private FilterDefinition<SongRating> CreateUserFilter(User user)
    {
        return Builders<SongRating>.Filter
            .Eq(rating => rating.UserId, user.Id);
    }

    private FilterDefinition<SongRating> CreateSongFilter(Song song)
    {
        return Builders<SongRating>.Filter
            .Eq(rating => rating.SongId, song.Id);
    }
}