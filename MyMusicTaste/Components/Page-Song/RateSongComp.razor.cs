using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
using MyMusicTaste.Database;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.Page_Song;

// TODO: Add option to change rating by keyboard
/// <summary>
/// A component that allows a signed-in user to rate a song, view their existing rating,
/// update it, or remove it from their collection.
/// </summary>
public partial class RateSongComp : ComponentBase
{
    /// <summary>
    /// The ID of the song to  rate.
    /// </summary>
    [Parameter] public string SongId { get; set; } = null!;
    
    [Inject] private IDbRepository<SongRating> _ratingRepository { get; set; } = null!;
    [Inject] private IIdentityProvider _identity { get; set; } = null!;
    [Inject] private ISongRatingListing _ratingListing { get; set; } = null!;
    
    private enum ComponentState { NotLoaded, NotLoggedIn, Unrated, Rated }
    private ComponentState _componentState = ComponentState.NotLoaded;
    private SongRating? _currentRating;

    private byte _initialInputValue;
    private byte _currentInputValue;

    protected override async Task OnInitializedAsync()
    {
        await LoadSignedUserRating();

        if (_currentRating != null)
        {
            _initialInputValue = _currentRating.IsRated ?
                _currentRating.Rating : (byte) 0;
            _currentInputValue = _initialInputValue;
        }
    }
    
    private async Task LoadSignedUserRating()
    {
        if (!_identity.IsAuthenticated())
        {
            _componentState = ComponentState.NotLoggedIn;
            return;
        }

        var signedUserId = _identity.GetUserId();
        if (signedUserId == null)
        {
            _componentState = ComponentState.NotLoggedIn;
            return;
        }
        
        _currentRating =  await _ratingListing.GetSongRatingAsync(SongId, signedUserId);
        _componentState = _currentRating == null ?
            ComponentState.Unrated : ComponentState.Rated;
        
        StateHasChanged();
    }

    private async Task AddRating()
    {
        if (_currentRating != null) return;
        _currentRating = new SongRating
        {
            UserId = new ObjectId(_identity.GetUserId()!),
            SongId = new ObjectId(SongId)
        };

        try
        {
            await _ratingRepository.CreateAsync(_currentRating);
            _componentState = ComponentState.Rated;
        }
        catch
        {
            _currentRating = null;
        }
    }

    private void ChangeRating(ChangeEventArgs args)
    {
        if (_currentRating == null) return;
        if (args.Value == null) return;

        _currentInputValue = Byte.Parse(args.Value.ToString()!);
        StateHasChanged();
    }
    
    private async Task SaveNewRating(ChangeEventArgs args)
    {
        if (_currentRating!.Rating == _currentInputValue) return;
        var prevRating = _currentRating;
        _currentRating!.Rating = _currentInputValue == 0 ?
            SongRating.NOT_RATED : _currentInputValue;
        
        try
        {
            await _ratingRepository.UpdateAsync(_currentRating);
        }
        catch
        {
            _currentRating = prevRating;
        }
    }
    
    private async Task RemoveRating()
    {
        if (_currentRating == null) return;
        await _ratingRepository.DeleteAsync(_currentRating!);
        _currentRating = null;
        _componentState = ComponentState.Unrated;
    }

    private string GetShownLabelValue()
    {
        return _currentInputValue == 0 ? 
            "Unrated" : _currentInputValue.ToString();
    }
}