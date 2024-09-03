using Microsoft.AspNetCore.Components;
using MongoDB.Driver.Search;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.SearchComps;

public partial class SongSearch : ComponentBase
{
    [Parameter]
    public string? Query { get; set; }
    
    [Parameter] 
    public int ResultsCount { get; set; }
    
    private List<Song>? _results { get; set; }
    
    public void UpdateResults()
    {
        if (Query == null)
        {
            _results = null;
            return;
        }
        
        _results = Searcher.Search(Query, ResultsCount);
        StateHasChanged();
    }

    protected override void OnAfterRender(bool _)
    {
        UpdateResults();
    }
}