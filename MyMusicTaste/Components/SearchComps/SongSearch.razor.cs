using Microsoft.AspNetCore.Components;
using MongoDB.Driver.Search;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.SearchComps;

public partial class SongSearch : ComponentBase
{
    [Parameter] 
    public int ResultsCount { get; set; }
    
    private List<Song> _results { get; set; }

    private void SearchSongs(string query)
    {
        _results = Searcher.Search(query, ResultsCount);
    }
}