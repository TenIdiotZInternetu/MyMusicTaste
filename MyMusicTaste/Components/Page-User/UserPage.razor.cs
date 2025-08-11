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

    [Parameter] public string UserId { get; set; } = null!;

    [Inject] private IDbRepository<User> _userRepository {get;set;} = null!;
    [Inject] private ISongRatingListing _ratingListing { get; set; } = null!;
    [Inject] private IIdentityProvider _identity {get;set;} = null!;
    
    private User? _user;
    private IEnumerable<SongRating>? _ratings;
    
    private enum PageState { Loading, Loaded, UserNotFound }
    private PageState _pageState = PageState.Loading;

    private bool _ownerAuthorized;

    private string _aboutMeText => GetShownAboutMeText();
    private string? _profilePicLink => GetShownProfilePic();
    
    private bool _inEditMode;
    private bool _saving;
    private string _saveBtnStyle => UnsavedChanges() ? "primary" : "secondary";

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
            _user = _userRepository.GetById(UserId);
            _ownerAuthorized = _identity.AuthorizeUserById(UserId);
            
            var unorderedRatings = await _ratingListing.GetRatingsByUserAsync(_user);
            _ratings = unorderedRatings.OrderByDescending(rating => 
                rating.Rating == SongRating.NOT_RATED ? -1 : rating.Rating);
            
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
        if (_tempProfilePicLink == _user!.ProfilePictureLink)  return;
        
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
        
        _user!.ProfilePictureLink = _tempProfilePicLink;
        _user!.AboutMe = _tempAboutMeText;

        await _userRepository.UpdateAsync(_user);
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
        };
        return _user!.AboutMe ??  "I'm a mysterious person";
    }
}