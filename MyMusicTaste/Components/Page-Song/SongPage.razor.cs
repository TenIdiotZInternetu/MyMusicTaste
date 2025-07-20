using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.Page_Song;

public partial class SongPage : ComponentBase
{
    public const string ROUTE_TEMPLATE = "/songs/{SongId}";
    
    [Parameter] public string? SongId { get; set; }
    
    private enum PageState { Loading, Loaded, SongNotFound }

    [Inject] private ISongStatsCalculation _statsCalculation { get; set; } = null!;

    private PageState _pageState = PageState.Loading;
    private Models.Song? _song;
    private SongStats? _stats;

    public static string GetRoute(ObjectId songId)
    {
        return ROUTE_TEMPLATE.Replace("{SongId}", songId.ToString());
    }
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            _song = SongRepository.GetById(SongId);
            _stats = await _statsCalculation.CalculateSongStats(_song);
            _pageState = PageState.Loaded;
            StateHasChanged();
        }
        catch (EntryNotFoundException)
        {
            _pageState = PageState.SongNotFound;
        }
    }
}