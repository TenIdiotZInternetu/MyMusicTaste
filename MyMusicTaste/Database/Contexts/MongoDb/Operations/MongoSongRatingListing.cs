using MongoDB.Bson;
using MongoDB.Driver;
using MyMusicTaste.Database.Connections;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

public class SongRatingListing : ISongRatingListing
{
    // Collection acquirement is subject to change
    private IMongoCollection<RatingDto> _collection = MongoDbContext.Client
        .GetDatabase("Core").GetCollection<RatingDto>("SongRatings");
        
    private class RatingDto : SongRating
    {
        public ObjectId UserId;
        public ObjectId SongId;
    }
    
    public IEnumerable<SongRating> GetRatingsByUser(User user)
    {
        var filter = Builders<RatingDto>.Filter
            .Eq(x => x.UserId, user.Id);

        List<RatingDto> results = _collection.Find(filter).ToList();
        return results;
    }

    public IEnumerable<SongRating> GetRaginsBySong(Song song)
    {
        throw new NotImplementedException();
    }
}