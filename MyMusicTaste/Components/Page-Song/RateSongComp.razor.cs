using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
using MyMusicTaste.Database;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.Page_Song;

public partial class RateSongComp : ComponentBase
{
    [Parameter] public string SongId { get; set; } = null!;
    
    [Inject] private IDbRepository<SongRating> _ratingRepository { get; set; } = null!;
    [Inject] private IIdentityProvider _identity { get; set; } = null!;
    [Inject] private ISongRatingListing _ratingListing { get; set; } = null!;
    
    private enum ComponentState { NotLoaded, NotLoggedIn, Unrated, Rated }
    private ComponentState _componentState = ComponentState.NotLoaded;
    private SongRating? _currentRating;

    private byte _initialInputValue;

    protected override async Task OnInitializedAsync()
    {
        await LoadSignedUserRating();

        if (_currentRating != null)
        {
            _initialInputValue = _currentRating.Rating == SongRating.NOT_RATED ?
                (byte) 0 : _currentRating.Rating;
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

    private async Task ChangeRating(ChangeEventArgs args)
    {
        if (_currentRating == null) return;
        if (args.Value == null) return;
        
        byte prevRating = _currentRating.Rating;
        byte inputValue = Byte.Parse(args.Value.ToString()!);

        _currentRating.Rating = inputValue == 0 ?
            SongRating.NOT_RATED : inputValue;
        
        try
        {
            await _ratingRepository.UpdateAsync(_currentRating);
        }
        catch
        {
            _currentRating.Rating = prevRating;
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
        if (_currentRating == null) return "Unrated";
        return _currentRating.Rating == SongRating.NOT_RATED ? 
            "Unrated" : _currentRating.Rating.ToString();
    }
}