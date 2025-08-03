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
    [Inject] private IIdentityProvider _identity { get; set; } = null!;
    
    private enum PageState { Loading, Loaded, SongNotFound }
    private PageState _pageState = PageState.Loading;
    
    private enum AddRatingState { NotLoaded, NotLoggedIn, Unrated, Rated }
    private AddRatingState _addRatingState = AddRatingState.NotLoaded;
    
    private Song? _song;
    private SongStats? _stats;
    private SongRating? _signedUserRating;
    
    private bool _statsCalculated;

    public static string GetRoute(ObjectId songId)
    {
        return ROUTE_TEMPLATE.Replace("{SongId}", songId.ToString());
    }
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            _song = SongRepository.GetById(SongId);
            _pageState = PageState.Loaded;
        }
        catch (EntryNotFoundException)
        {
            _pageState = PageState.SongNotFound;
            return;
        }

        Task.WaitAll(
            LoadStats(),
            LoadSignedUserRating()
        );
    }

    private async Task LoadStats()
    {
        _stats = await _statsCalculation.CalculateSongStatsAsync(_song!);
        _statsCalculated = true;
        StateHasChanged();
    }

    private async Task LoadSignedUserRating()
    {
        if (!_identity.IsAuthenticated()) return;

        var signedUserId = _identity.GetUserId();
        if (signedUserId == null)
        {
            _addRatingState = AddRatingState.NotLoggedIn;
            return;
        }
        
        _signedUserRating =  await _ratingListing.GetSongRatingAsync(SongId, signedUserId);
        _addRatingState = _signedUserRating == null ?
            AddRatingState.Unrated : AddRatingState.Rated;
        
        StateHasChanged();
    }
}