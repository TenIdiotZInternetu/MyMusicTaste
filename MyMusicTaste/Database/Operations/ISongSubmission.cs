using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Operations;

public interface ISongSubmission
{
    public Task<string> SubmitSongAsync(Song songModel);
}