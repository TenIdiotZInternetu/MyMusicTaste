using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Operations;

/// <summary>
/// Provides a generic interface for searching entities of type <typeparamref name="TModel"/>.
/// </summary>
/// <typeparam name="TModel">The type of model to search for.</typeparam>
public interface ISearchOperation<TModel> where TModel : Model
{
    /// <summary>
    /// Searches entities based on a query string.
    /// </summary>
    /// <param name="query">The search query string.</param>
    /// <param name="resultsCount">The maximum number of results to return.</param>
    /// <returns>A task for the collection of matching entities, or null if the query is empty.</returns>
    public Task<IEnumerable<TModel>?> SearchAsync(string query, int resultsCount);
}