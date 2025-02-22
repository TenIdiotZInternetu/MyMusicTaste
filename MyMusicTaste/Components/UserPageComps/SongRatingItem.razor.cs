using Microsoft.AspNetCore.Components;
using MyMusicTaste.Components.Pages;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.UserPageComps;

public partial class SongRatingItem : ComponentBase {
    [Parameter]
    public SongRating? Rating { get; set; } = new();
    public Song Song => Rating.Song;
    
    protected override void OnParametersSet()
    {
        ;
    }

    private void GoToSongPage()
    {
        Navigation.NavigateTo(SongPage.GetRoute(Song!.Id));
    }
}