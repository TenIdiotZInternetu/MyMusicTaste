using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Operations;

/// <summary>
/// Provides methods for calculating statistics for a song, such as average rating, median rating, total listens, and rating distribution.
/// </summary>
public interface ISongStatsCalculation
{
    /// <summary>
    /// Calculates statistics for a song.
    /// </summary>
    /// <param name="song">The song to calculate statistics for.</param>
    /// <returns>A task for the song's statistics.</returns>
    public Task<SongStats> CalculateSongStatsAsync(Song song);
}