using Microsoft.AspNetCore.Components;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.Page_Search;

/// <summary>
/// Base component for search pages. Provides shared search functionality for a given model type.
/// </summary>
/// <typeparam name="TModel">The type of the model to search for, must inherit from Model.</typeparam>
public class SearchComponent<TModel> : ComponentBase
    where TModel : Model
{
    /// <summary>
    /// The search query string entered by the user.
    /// </summary>
    [Parameter] public string? Query { get; set; }
    
    /// <summary>
    /// Maximum number of results to return.
    /// </summary>
    [Parameter] public int ResultsCount { get; set; }

    /// <summary>
    /// The search operation service used to perform the query.
    /// </summary>
    [Inject] protected ISearchOperation<TModel> Searcher { get; set; } = null!;
    
    /// <summary>
    /// The list of search results returned by the query.
    /// </summary>
    protected IEnumerable<TModel>? Results { get; set; }
    
    /// <summary>
    /// Performs the search with the current query and updates the <see cref="Results"/>.
    /// </summary>
    public async Task UpdateResults()
    {
        if (Query == null)
        {
            Results = null;
            return;
        }
        
        Results = await Searcher.SearchAsync(Query, ResultsCount);
        StateHasChanged();
    }

    protected override async Task OnAfterRenderAsync(bool _)
    {
        await UpdateResults();
    }
}