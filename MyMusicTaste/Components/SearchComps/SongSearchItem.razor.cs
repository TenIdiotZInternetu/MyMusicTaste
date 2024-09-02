using Microsoft.AspNetCore.Components;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.SearchComps;

public partial class SongSearchItem : ComponentBase
{
    [Parameter]
    public Song? Song {get; set;}
    
    private bool _hasCover => Song.CoverImageLink != null;

    private void GoToSongPage()
    {
    }
}