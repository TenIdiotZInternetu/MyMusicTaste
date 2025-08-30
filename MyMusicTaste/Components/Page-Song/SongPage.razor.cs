using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
using MyMusicTaste.Database;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.Page_Song;

public partial class SongPage : ComponentBase
{
    public const string ROUTE_TEMPLATE = "/songs/{SongId}";

    [Parameter] public string SongId { get; set; } = null!;
    
    [Inject] private ISongStatsCalculation _statsCalculation { get; set; } = null!;
    [Inject] private ISongRatingListing _ratingListing { get; set; } = null!;
    [Inject] private IDbRepository<Song> _songRepository { get; set; } = null!;
    
    private enum PageState { Loading, Loaded, SongNotFound }
    private PageState _pageState = PageState.Loading;
    
    private Song? _song;
    private SongStats? _stats;
    
    private bool _statsCalculated;

    public static string GetRoute(string songId)
    {
        return ROUTE_TEMPLATE.Replace("{SongId}", songId);
    }
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            _song = _songRepository.GetById(SongId);
            _pageState = PageState.Loaded;
            await LoadStats();
        }
        catch (EntryNotFoundException)
        {
            _pageState = PageState.SongNotFound;
        }
    }

    private async Task LoadStats()
    {
        _stats = await _statsCalculation.CalculateSongStatsAsync(_song!);
        _statsCalculated = true;
        StateHasChanged();
    }
}