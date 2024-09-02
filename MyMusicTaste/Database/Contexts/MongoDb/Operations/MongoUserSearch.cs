using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

public class MongoUserSearch : ISearchOperation<User>
{
    public List<User>? Search(string query, int resultsCount)
    {
        throw new NotImplementedException();
    }
}