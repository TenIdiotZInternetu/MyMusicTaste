using Microsoft.AspNetCore.Components;
using MyMusicTaste.Components.Page_Song;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.Page_User;

public partial class SongRatingItem : ComponentBase {
    [Parameter]
    public SongRating? Rating { get; set; } = new();
    public Song? Song => Rating?.Song;
    
    protected override void OnParametersSet()
    {
        ;
    }

    private void GoToSongPage()
    {
        Navigation.NavigateTo(SongPage.GetRoute(Song!.Id));
    }
}