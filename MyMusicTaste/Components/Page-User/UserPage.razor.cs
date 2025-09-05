using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MyMusicTaste.Components.Dialogs;
using MyMusicTaste.Database;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;
using MyMusicTaste.Utils;

namespace MyMusicTaste.Components.Page_User;

// TODO: Decompose this page
/// <summary>
/// Displays a user's profile including general info, statistics, and comments.
/// Supports editing the profile for the authorized user.
/// </summary>
public partial class UserPage : ComponentBase
{
    public const string ROUTE_TEMPLATE = "/users/{UserId}";

    /// <summary>
    /// The ID of the user whose profile is being displayed.
    /// </summary>
    [Parameter] public string UserId { get; set; } = null!;

    [Inject] private IDbRepository<User> _userRepository { get; set; } = null!;
    [Inject] private IIdentityProvider _identity { get; set; } = null!;
    [Inject] private IUserStatsCalculation _statsCalculation { get; set; } = null!;
    
    private enum PageState { Loading, Loaded, UserNotFound }
    private PageState _pageState = PageState.Loading;
    
    private enum Tab { Songs, Albums, Authors, Genres }
    private Tab _currentTab = Tab.Songs;
    
    private User? _user;
    private UserStats? _userStats;
    
    private bool _ownerAuthorized;
    private bool _inEditMode;
    private bool _saving;
    
    private string? _tempProfilePicLink;
    private string? _tempAboutMeText;

    private string _aboutMeText => GetShownAboutMeText();
    private string? _profilePicLink => GetShownProfilePic();
    private string _saveBtnStyle => UnsavedChanges() ? "primary" : "secondary";

    private ConfirmDialog _unsavedChangesDialog = null!;
    private InputDialog _pictureLinkDialog = null!;
    
    public static string GetRoute(ObjectId userId)
    {
        return ROUTE_TEMPLATE.Replace("{UserId}", userId.ToString());
    }
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            _user = await _userRepository.GetByIdAsync(UserId);
            _userStats = await _statsCalculation.CalculateUserStatsAsync(UserId);
            
            _ownerAuthorized = _identity.AuthorizeUserById(UserId);
            _pageState = PageState.Loaded;
        }
        catch (EntryNotFoundException)
        {
            _pageState = PageState.UserNotFound;
        }
    }

    private void OpenEditMode()
    {
        if (!_ownerAuthorized) return;
        _inEditMode = true;
    }

    private async Task CloseEditMode()
    {
        if (!_ownerAuthorized) return;
        if (!_inEditMode) return;
        
        bool shouldClose = true;
        
        if (UnsavedChanges())
        {
            var result = await _unsavedChangesDialog.OpenDialog();
            shouldClose = result.WasConfirmed;
        }

        if (!shouldClose) return;

        _tempAboutMeText = null;
        _tempProfilePicLink = null;
        _inEditMode = false;
        StateHasChanged();
    }

    private async Task ChangeProfilePicture()
    {
        if (!_ownerAuthorized) return;
        if (!_inEditMode) return;
        
        var result = await _pictureLinkDialog.OpenDialog();
        if (!result.WasConfirmed) return;
        
        if (await LinkValidation.IsImageLinkValidAsync(result.Result))
        {
            _tempProfilePicLink = result.Result;
            StateHasChanged();
        }
    }

    private void ChangeAboutMe(ChangeEventArgs args)
    {
        if (!_ownerAuthorized) return;
        if (!_inEditMode) return;

        _tempAboutMeText = args.Value?.ToString();
        StateHasChanged();
    }

    private async Task SaveChanges()
    {
        if (!_ownerAuthorized) return;
        if (!UnsavedChanges()) return;
        
        _saving = true;
        StateHasChanged();

        if (!_tempProfilePicLink.IsNullOrEmpty())
        {
            _user!.ProfilePictureLink = _tempProfilePicLink;
        }
        if (!_tempAboutMeText.IsNullOrEmpty())
        {
            _user!.AboutMe = _tempAboutMeText;
        }

        await _userRepository.UpdateAsync(_user!);
        _saving = false;
        StateHasChanged();
    }

    private bool UnsavedChanges()
    {
        return (_tempProfilePicLink != null && _tempProfilePicLink != _user!.ProfilePictureLink) ||
               (_tempAboutMeText != null && _tempAboutMeText != _user!.AboutMe);
    }

    private string? GetShownProfilePic()
    {
        return (_inEditMode && _tempProfilePicLink != null) ?
            _tempProfilePicLink : _user!.ProfilePictureLink;
    }

    private string GetShownAboutMeText()
    {
        if (_inEditMode && _tempAboutMeText != null)
        {
            return _tempAboutMeText;
        }
        return _user!.AboutMe ??  "I'm a mysterious person";
    }

    private List<(string, double)>? GetShownStats()
    {
        if (_userStats == null) return null;
        return _currentTab switch
        {
            Tab.Albums => _userStats.AlbumsByMean,
            Tab.Authors => _userStats.AuthorsByMean,
            Tab.Genres => _userStats.GenresByMean,
            _ => null
        };
    }
    
    private string TabIsActive(Tab tab)
    {
        return tab == _currentTab ? "active" : "";
    }
    
    private void ChangeTabToSongs() => _currentTab = Tab.Songs;
    private void ChangeTabToAlbums() => _currentTab = Tab.Albums;
    private void ChangeTabToAuthors() => _currentTab = Tab.Authors;
    private void ChangeTabToGenres() => _currentTab = Tab.Genres;
}