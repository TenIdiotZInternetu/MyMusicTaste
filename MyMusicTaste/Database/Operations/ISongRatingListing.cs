using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Operations;

public interface ISongRatingListing
{
    public Task<IEnumerable<SongRating>> GetRatingsByUserAsync(User user);
    public Task<IEnumerable<SongRating>> GetRatingsBySongAsync(Song song);
}