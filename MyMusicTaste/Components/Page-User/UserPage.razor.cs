using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
using MyMusicTaste.Components.Dialogs;
using MyMusicTaste.Database;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;
using MyMusicTaste.Utils;

namespace MyMusicTaste.Components.Page_User;

public partial class UserPage : ComponentBase
{
    public const string ROUTE_TEMPLATE = "/users/{UserId}";
    
    [Parameter] public string? UserId { get; set; }

    private User? _user;
    private IEnumerable<SongRating>? _ratings;
    
    private enum PageState { Loading, Loaded, UserNotFound }
    private PageState _pageState = PageState.Loading;

    private string _aboutMeText => GetShownAboutMeText();
    private string? _profilePicLink => GetShownProfilePic();
    
    private bool _inEditMode;
    private bool _unsavedChanges;
    private bool _saving;
    private string _saveBtnStyle => _unsavedChanges ? "primary" : "secondary";

    private ConfirmDialog _unsavedChangesDialog = null!;
    private InputDialog _pictureLinkDialog = null!;
    
    private string? _tempProfilePicLink;
    private string? _tempAboutMeText;
    
    public static string GetRoute(ObjectId userId)
    {
        return ROUTE_TEMPLATE.Replace("{UserId}", userId.ToString());
    }
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            _user = UserRepository.GetById(UserId);
            _ratings = await RatingListing.GetRatingsByUserAsync(_user);
            _pageState = PageState.Loaded;
        }
        catch (EntryNotFoundException)
        {
            _pageState = PageState.UserNotFound;
        }
    }

    private void OpenEditMode()
    {
        _inEditMode = true;
    }

    private async Task CloseEditMode()
    {
        bool shouldClose = true;
        
        if (_unsavedChanges)
        {
            var result = await _unsavedChangesDialog.OpenDialog();
            shouldClose = result.WasConfirmed;
        }

        if (!shouldClose) return;

        _tempAboutMeText = null;
        _tempProfilePicLink = null;
        _unsavedChanges = false;
        _inEditMode = false;
        StateHasChanged();
    }

    private async Task ChangeProfilePicture()
    {
        var result = await _pictureLinkDialog.OpenDialog();
        if (!result.WasConfirmed) return;
        if (await LinkValidation.IsImageLinkValidAsync(result.Result))
        {
            _tempProfilePicLink = result.Result;
        }

        _unsavedChanges = true;
    }

    private async Task SaveChanges()
    {
        if (!_unsavedChanges) return;
        
        _saving = true;
        StateHasChanged();
        
        _user!.ProfilePictureLink = _tempProfilePicLink;
        _user!.AboutMe = _tempAboutMeText;

        await UserRepository.UpdateAsync(_user);
        _unsavedChanges = false;
        _saving = false;
        StateHasChanged();
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
        };
        return _user!.AboutMe ??  "I'm a mysterious person.";
    }
}