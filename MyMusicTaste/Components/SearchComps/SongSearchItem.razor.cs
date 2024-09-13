using Microsoft.AspNetCore.Components;
using MyMusicTaste.Components.Pages;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.SearchComps;

public partial class SongSearchItem : ComponentBase
{
    [Parameter] 
    public Song? Song { get; set; } = new();

    private void GoToSongPage()
    {
        Navigation.NavigateTo(SongPage.GetRoute(Song!.Id));
    }
}