using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

public class SongRatingListing : ISongRatingListing
{
    public IEnumerable<SongRating> GetRatingsByUser(User user)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<SongRating> GetRaginsBySong(Song song)
    {
        throw new NotImplementedException();
    }
}