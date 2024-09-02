using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

public class UserSearch : ISearchOperation<User>
{
    public Task<IEnumerable<User>> Search(string query, int resultsCount)
    {
        throw new NotImplementedException();
    }
}