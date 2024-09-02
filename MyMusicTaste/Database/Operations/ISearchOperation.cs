using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Operations;

public interface ISearchOperation<TModel> where TModel : Model
{
    public List<TModel>? Search(string query, int resultsCount);
}