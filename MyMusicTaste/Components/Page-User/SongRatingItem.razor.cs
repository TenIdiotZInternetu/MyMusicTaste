using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.IdentityModel.Tokens;
using MyMusicTaste.Components.Page_Song;
using MyMusicTaste.Database;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.Page_User;

public partial class SongRatingItem : ComponentBase {
    [Parameter] public SongRating Rating { get; set; } = new();
    [Parameter] public bool Editable { get; set; }
    
    [Parameter] public EventCallback OnRatingSaved { get; set; }
    
    [Inject] private IDbRepository<Song> _songRepo { get; set; } = null!;
    [Inject] private IDbRepository<SongRating> _ratingsRepo { get; set; } = null!;
    [Inject] private IIdentityProvider _identity { get; set; } = null!;

    private enum ComponentState { Loading, Loaded, NotFound }
    private ComponentState _componentState = ComponentState.Loading;
    
    private Song? _song;
    private string _shownValue => Rating.IsRated ?
        Rating.Rating.ToString() : "-";

    private byte _tempRating;
    
    protected override void OnInitialized()
    {
        try
        {
            // TODO: Ineffective, should retrieve songs in bulk
            _song = _songRepo.GetById(Rating.SongId);
            _componentState = ComponentState.Loaded;
            ResetTempRating();
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

    private void ChangeRating(ChangeEventArgs args)
    {
        var value = args.Value!.ToString();
        if (value.IsNullOrEmpty()) return;
        
        _tempRating = Byte.Parse(value!);
    }

    private void ResetTempRating()
    {
        _tempRating = Rating.Rating;
        StateHasChanged();
    }
    
    private async Task SaveRatingAsync()
    {
        Rating.Rating = _tempRating;
        await _ratingsRepo.UpdateAsync(Rating);
        await OnRatingSaved.InvokeAsync();
        StateHasChanged();
    }

    private async Task HandleKeyAsync(KeyboardEventArgs args)
    {
        if (args.Key == "Enter")
        {
            await SaveRatingAsync();
        }

        if (args.Key == "Escape")
        {
            ResetTempRating();
        }
    }
}