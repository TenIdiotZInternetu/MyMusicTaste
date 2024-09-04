using MongoDB.Bson;
using MongoDB.Driver;
using MyMusicTaste.Database.Connections;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

public class MongoSongRatingListing : ISongRatingListing
{
    private class RatingDto : SongRating
    {
        public ObjectId UserId;
        public ObjectId SongId;
    }
    
    // Collection acquirement is subject to change
    private IMongoCollection<RatingDto> _collection = MongoDbContext.Client
        .GetDatabase("Core").GetCollection<RatingDto>("SongRatings");

    private IDbRepository<User> _usersRepo;
    private IDbRepository<Song> _songsRepo;

    public MongoSongRatingListing(IDbRepository<User> usersRepo, IDbRepository<Song> songsRepo)
    {
        _usersRepo = usersRepo;
        _songsRepo = songsRepo;
    }

    public IEnumerable<SongRating> GetRatingsByUser(User user)
    {
        var filter = Builders<RatingDto>.Filter
            .Eq(x => x.UserId, user.Id);

        List<RatingDto> results = _collection.Find(filter).ToList();
        return results.ToList().Select(dto => new SongRating()
        {
            Id = dto.Id,
            User = user,
            Song = _songsRepo.GetById(dto.SongId),
            Rating = dto.Rating
        });
    }

    public IEnumerable<SongRating> GetRaginsBySong(Song song)
    {
        throw new NotImplementedException();
    }
}