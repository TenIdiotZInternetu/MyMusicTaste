using Microsoft.AspNetCore.Components;
using MongoDB.Driver.Search;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.SearchComps;

public class SearchComponent<TModel> : ComponentBase
    where TModel : Model
{
    [Parameter]
    public string? Query { get; set; }
    
    [Parameter] 
    public int ResultsCount { get; set; }

    [Inject] 
    protected ISearchOperation<TModel>? Searcher { get; set; }
    
    protected IEnumerable<TModel>? Results { get; set; }
    
    public async Task UpdateResults()
    {
        if (Query == null)
        {
            Results = null;
            return;
        }

        if (Searcher == null)
        {
            throw new NullReferenceException("Injected ISearchOperation is null");
        }
        
        Results = await Searcher.SearchAsync(Query, ResultsCount);
        StateHasChanged();
    }

    protected override async Task OnAfterRenderAsync(bool _)
    {
        await UpdateResults();
    }
}