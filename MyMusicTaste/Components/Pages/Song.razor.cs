using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
using MongoDB.Driver;
using MyMusicTaste.Database.Models;
using MyMusicTaste.Database.Operations;

namespace MyMusicTaste.Components.Pages;

public partial class Song : ComponentBase
{
    private enum PageState { Loading, Loaded, SongNotFound}
    
    [Parameter]
    public string? SongId { get; set; }
    
    private SongFullModel? _song { get; set; }
    private PageState _pageState { get; set; } = PageState.Loading;
    
    protected override void OnInitialized()
    {
        try
        {
            _song = GenericDbOperations.GetModelById<SongFullModel>(SongId);
            _pageState = PageState.Loaded;
        }
        catch (EntryNotFoundException e)
        {
            _pageState = PageState.SongNotFound;
        }
    }
}