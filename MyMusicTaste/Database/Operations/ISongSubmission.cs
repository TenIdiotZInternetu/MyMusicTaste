using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Operations;

/// <summary>
/// Provides an abstraction for submitting new songs to the database while avoiding duplicates.
/// </summary>
public interface ISongSubmission
{
    /// <summary>
    /// Submits a new song.
    /// </summary>
    /// <param name="songModel">The song to submit.</param>
    /// <returns>A task for the ID of the newly created song as a string.</returns>
    /// <exception cref="EntryAlreadyExistsException">Thrown if the song already exists.</exception>
    public Task<string> SubmitSongAsync(Song songModel);
}