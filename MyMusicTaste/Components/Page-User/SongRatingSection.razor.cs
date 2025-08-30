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
    
    private IEnumerable<SongRating>? _ratingItems;

    private SongRating? _draggedItem;
    private Dropzone? _activeDropzone;

    protected override async Task OnInitializedAsync()
    {
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

    private void StopDragging()
    {
        _draggedItem = null;
        _activeDropzone?.SetActive(false);
        _activeDropzone = null;
    }

    private async Task SaveDraggedItem(int newRating)
    {
        if (_draggedItem == null) return;
        _draggedItem.Rating = (byte)newRating;
        await _ratingRepo.UpdateAsync(_draggedItem);
        
        _draggedItem = null;
        ReorderRatings();
        StateHasChanged();
    }

    private void ChangeDropzone(Dropzone newDropzone)
    {
        _activeDropzone?.SetActive(false);
        _activeDropzone = newDropzone;
    }
}