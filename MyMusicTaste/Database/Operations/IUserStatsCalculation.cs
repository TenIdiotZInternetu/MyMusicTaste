using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Operations;

public interface IUserStatsCalculation
{
    public Task<UserStats> CalculateUserStatsAsync(string userId);
}