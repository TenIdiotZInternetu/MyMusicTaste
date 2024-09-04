using MyMusicTaste.Models;

namespace MyMusicTaste.Algorithms.Stats;

public class SongStatsCalculation
{
    private Song _song { get; init; }
    private IEnumerable<SongRating> _ratings { get; init; }

    public SongStatsCalculation(Song song, IEnumerable<SongRating> ratings)
    {
        _song = song;
        _ratings = ratings;
    }

    public float OverallRating()
    {
        throw new NotImplementedException();
    }

    public int RatingCount()
    {
        throw new NotImplementedException();
    }
}