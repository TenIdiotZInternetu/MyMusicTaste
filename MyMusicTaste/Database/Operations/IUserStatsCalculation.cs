using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Operations;

/// <summary>
/// Provides methods for calculating statistics for a user based on their song ratings.
/// </summary>
public interface IUserStatsCalculation
{
    /// <summary>
    /// Calculates statistics for a user, such as average ratings grouped by album, author, and genre.
    /// </summary>
    /// <param name="userId">The ID of the user to calculate statistics for.</param>
    /// <returns>A task for the user's statistics.</returns>
    public Task<UserStats> CalculateUserStatsAsync(string userId);
}