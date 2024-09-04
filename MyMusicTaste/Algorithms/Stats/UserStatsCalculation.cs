using MyMusicTaste.Models;

namespace MyMusicTaste.Algorithms.Stats;

public class UserStatsCalculation
{
    private User _user { get; init; }
    private IEnumerable<SongRating> _ratings { get; init; }

    public UserStatsCalculation(User user, IEnumerable<SongRating> ratings)
    {
        _user = user;
        _ratings = ratings;
    }

    public List<string> AlbumsByTop()
    {
        throw new NotImplementedException();
    }

    public List<string> AlbumsByAverage()
    {
        throw new NotImplementedException();
    }

    public List<string> AuthorsByTop()
    {
        throw new NotImplementedException();
    }

    public List<string> AuthorsByAverage()
    {
        throw new NotImplementedException();
    }

    public List<string> GenresByAverage()
    {
        throw new NotImplementedException();
    }
}