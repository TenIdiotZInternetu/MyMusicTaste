using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Operations;

public interface ISearchOperation<TModel> where TModel : Model
{
    public Task<IEnumerable<TModel>> Search(string query, int resultsCount);
}