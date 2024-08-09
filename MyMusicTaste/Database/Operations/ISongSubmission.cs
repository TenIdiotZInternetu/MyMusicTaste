using MongoDB.Driver;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Operations;

public interface ISongSubmission
{
    public void SubmitSong(Song songModel);
    public Task SubmitSongAsync(Song song);
}