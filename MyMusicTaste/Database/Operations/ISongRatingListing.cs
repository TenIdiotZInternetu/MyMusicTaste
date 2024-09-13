using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Operations;

public interface ISongRatingListing
{
    public IEnumerable<SongRating> GetRatingsByUser(User user);
    public IEnumerable<SongRating> GetRatingsBySong(Song song);
}