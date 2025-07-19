using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Operations;

public interface ISongStatsCalculation
{
    public Task<SongStats> CalculateSongStats(Song song);
}