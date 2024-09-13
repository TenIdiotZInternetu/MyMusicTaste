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
    
    protected List<TModel>? Results { get; set; }
    
    public void UpdateResults()
    {
        if (Query == null)
        {
            Results = null;
            return;
        }
        
        Results = Searcher.Search(Query, ResultsCount);
        StateHasChanged();
    }

    protected override void OnAfterRender(bool _)
    {
        UpdateResults();
    }
}