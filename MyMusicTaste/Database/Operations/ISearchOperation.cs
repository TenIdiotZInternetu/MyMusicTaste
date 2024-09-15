using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Operations;

public interface ISearchOperation<TModel> where TModel : Model
{
    public Task<IEnumerable<TModel>?> SearchAsync(string query, int resultsCount);
}