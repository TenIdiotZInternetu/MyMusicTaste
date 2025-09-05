using Microsoft.AspNetCore.Components;
using MyMusicTaste.Database;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.Page_Song;

/// <summary>
/// DPage for displaying detailed information about a song, including its metadata, statistics, and comments.
/// </summary>
public partial class SongPage : ComponentBase
{
    public const string ROUTE_TEMPLATE = "/songs/{SongId}";

    /// <summary>
    /// The ID of the song to display.
    /// </summary>
    [Parameter] public string SongId { get; set; } = null!;
    
    [Inject] private ISongStatsCalculation _statsCalculation { get; set; } = null!;
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

    private int[] CreateHistogramLabels()
    {
        int[] labels = new int[11];
        for (int i = 0; i < labels.Length; i++)
        {
            labels[i] = i * 10;
        }
        return labels;
    }
}