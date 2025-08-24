using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Operations;

public interface ISongRatingListing
{
    public Task<SongRating> GetSongRatingAsync(string songId, string userId);
    public Task<IEnumerable<SongRating>> GetRatingsByUserAsync(string userId);
    public Task<IEnumerable<SongRating>> GetRatingsBySongAsync(string songId);
}