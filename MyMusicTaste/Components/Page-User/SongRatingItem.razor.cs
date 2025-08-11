using Microsoft.AspNetCore.Components;
using MyMusicTaste.Components.Page_Song;
using MyMusicTaste.Database;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.Page_User;

public partial class SongRatingItem : ComponentBase {
    [Parameter] public SongRating Rating { get; set; } = new();
    
    [Inject] private IDbRepository<Song> _songRepo { get; set; } = null!;

    private enum ComponentState { Loading, Loaded, NotFound }
    private ComponentState _componentState = ComponentState.Loading;
    
    private Song? _song;
    private string _shownValue => Rating.Rating == SongRating.NOT_RATED ?
        "-" : Rating.Rating.ToString();
    
    protected override Task OnInitializedAsync()
    {
        try
        {
            // TODO: Ineffective, should retrieve songs in bulk
            _song = _songRepo.GetById(Rating.SongId);
            _componentState = ComponentState.Loaded;
        }
        catch (EntryNotFoundException)
        {
            _componentState = ComponentState.NotFound;
        }
    }

    private void GoToSongPage()
    {
        Navigation.NavigateTo(SongPage.GetRoute(_song!.Id));
    }
}