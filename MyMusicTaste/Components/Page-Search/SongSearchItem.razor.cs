using Microsoft.AspNetCore.Components;
using MyMusicTaste.Components.Page_Song;

namespace MyMusicTaste.Components.Page_Search;

public partial class SongSearchItem : ComponentBase
{
    [Parameter] public Models.Song? Song { get; set; } = new();

    private void GoToSongPage()
    {
        Navigation.NavigateTo(SongPage.GetRoute(Song!.Id.ToString()));
    }
}