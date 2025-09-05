using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Operations;

/// <summary>
/// Provides methods for retrieving song ratings from the database.
/// </summary>
public interface ISongRatingListing
{
    /// <summary>
    /// Retrieves the rating a specific user has given to a specific song.
    /// </summary>
    /// <param name="songId">The ID of the song.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task for the retrieved song rating, or null if no rating exists.</returns>
    public Task<SongRating> GetSongRatingAsync(string songId, string userId);
    
    /// <summary>
    /// Retrieves all ratings submitted by a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task for the list of ratings.</returns>
    public Task<IEnumerable<SongRating>> GetRatingsByUserAsync(string userId);
    
    /// <summary>
    /// Retrieves all ratings associated with a specific song.
    /// </summary>
    /// <param name="songId">The ID of the song.</param>
    /// <returns>A task for the list of ratings.</returns>
    public Task<IEnumerable<SongRating>> GetRatingsBySongAsync(string songId);
}