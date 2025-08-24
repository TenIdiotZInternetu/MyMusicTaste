using Microsoft.AspNetCore.Components;
using MyMusicTaste.Database;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.Page_User;

public partial class SongRatingSection : ComponentBase
{
    [Parameter] public string UserId { get; set; } = null!;
    
    [Inject] private ISongRatingListing _ratingListing { get; set; } = null!;
    
    private IEnumerable<SongRating>? _ratings;

    protected override async Task OnInitializedAsync()
    {
        _ratings = await _ratingListing.GetRatingsByUserAsync(UserId);
        ReorderRatings();
    }

    private void ReorderRatings()
    {
        _ratings = _ratings!.OrderByDescending(rating => 
            rating.IsRated ? rating.Rating : -1
        );
        
        StateHasChanged();
    }
}