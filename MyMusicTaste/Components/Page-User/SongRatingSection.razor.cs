using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MyMusicTaste.Components.Misc;
using MyMusicTaste.Database;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.Page_User;

public partial class SongRatingSection : ComponentBase
{
    [Parameter] public string UserId { get; set; } = null!;
    
    [Inject] private ISongRatingListing _ratingListing { get; set; } = null!;
    [Inject] private IDbRepository<SongRating> _ratingRepo { get; set; } = null!;
    [Inject] private IIdentityProvider _identity { get; set; } = null!;
    
    private IEnumerable<SongRating>? _ratingItems;

    private bool _userAuthorized;
    private SongRating? _draggedItem;
    private Dropzone? _activeDropzone;
    private int _newRatingOnDrop;
    
    private bool _isDragging => _draggedItem != null;
    private string _itemZValue => _isDragging ? "z-n1" : "z-1";

    protected override async Task OnInitializedAsync()
    {
        _userAuthorized = _identity.AuthorizeUserById(UserId);
        _ratingItems = await _ratingListing.GetRatingsByUserAsync(UserId);
        ReorderRatings();
    }

    private void ReorderRatings()
    {
        _ratingItems = _ratingItems!.OrderByDescending(rating => 
            rating.IsRated ? rating.Rating : -1
        );
        
        StateHasChanged();
    }

    private void StartDragging(SongRating item)
    {
        _draggedItem = item;
    }

    private async Task StopDragging()
    {
        if (_draggedItem == null) return;
        if (_newRatingOnDrop != _draggedItem.Rating)
        {
            await UpdateDraggedItem();
        }
        
        _draggedItem = null;
        _activeDropzone?.SetActive(false);
        _activeDropzone = null;
        _newRatingOnDrop = 0;
    }

    private async Task UpdateDraggedItem()
    {
        if (_draggedItem == null) return;
        _draggedItem.Rating = (byte)_newRatingOnDrop;
        await _ratingRepo.UpdateAsync(_draggedItem);
        
        _draggedItem = null;
        ReorderRatings();
        StateHasChanged();
    }

    private void ChangeDropzone(Dropzone newDropzone, int ratingOnDrop)
    {
        _activeDropzone?.SetActive(false);
        _activeDropzone = newDropzone;
        _newRatingOnDrop = ratingOnDrop;
    }
}