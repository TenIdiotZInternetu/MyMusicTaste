using Microsoft.AspNetCore.Components;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MyMusicTaste.Components.Dialogs;
using MyMusicTaste.Components.Page_User;
using MyMusicTaste.Database;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.Comments;

//TODO: Add a way to display user's Song Rating (or some other metric) on particular page types
public partial class CommentComp : ComponentBase
{
    [Parameter] public Comment Comment { get; set; } = null!;
    // TODO: Split Unposted Comments into its own component to avoid coupling
    [Parameter] public bool IsUnposted { get; set; }

    public event Action? OnEditModeOpened;
    public event Action<bool>? OnEditModeClosed;
    public event Action? OnChangesSaved;
    public event Action? OnCommentDeleted;

    [Inject] private IDbRepository<User> _userRepo { get; set; } = null!;
    [Inject] private IDbRepository<Comment> _commentRepo { get; set; } = null!;
    [Inject] private IIdentityProvider _identity { get; set; } = null!;
    
    private NavigationManager _navigationManager { get; set; } = null!;
    
    private enum CompState { NotLoaded, Loaded, Deleted }
    private CompState _state = CompState.NotLoaded;
    
    private User? _poster;
    private bool _ownedByUser;
    
    private bool _inEditMode;
    private string? _tempContent;

    private string _content => GetShownContent();
    private bool _showLastEdit => Comment.LastEditTimeValid;
    private string _saveBtnStyle => UnsavedChanges() ? "primary" : "secondary";
    
    private ConfirmDialog _unsavedChangesDialog = null!;
    private ConfirmDialog _deleteCommentDialog = null!;
    
    protected override async Task OnInitializedAsync()
    {
        _poster = await _userRepo.GetByIdAsync(Comment.UserId.ToString());
        _ownedByUser = _identity.AuthorizeUserById(_poster.Id.ToString());
        _inEditMode = IsUnposted;
        _state = CompState.Loaded;
    }

    private void OpenEditMode()
    {
        if (!_ownedByUser) return;
        _inEditMode = true;
        StateHasChanged();
        OnEditModeOpened?.Invoke();
    }

    private async Task CloseEditMode()
    {
        if (!_ownedByUser) return;
        if (!_inEditMode) return;
        
        bool shouldClose = true;
        
        if (UnsavedChanges())
        {
            var result = await _unsavedChangesDialog.OpenDialog();
            shouldClose = result.WasConfirmed;
        }

        if (!shouldClose) return;
        
        _tempContent = null;
        _inEditMode = false;
        StateHasChanged();
        OnEditModeClosed?.Invoke(!UnsavedChanges());
    }

    private void ChangeContent(ChangeEventArgs args)
    {
        if (!_ownedByUser) return;
        if (!_inEditMode) return;
        
        _tempContent = args.Value?.ToString();
        StateHasChanged();
    }

    private async Task SaveChangesAsync()
    {
        if (!_ownedByUser) return;
        if (!UnsavedChanges()) return;

        if (IsUnposted)
        {
            Comment.CreationTime = DateTime.Now;
            await _commentRepo.CreateAsync(Comment);
        }
        else
        {
            Comment.LastEditTime = DateTime.Now;
            await _commentRepo.UpdateAsync(Comment);
            IsUnposted = false;
        }
        
        OnChangesSaved?.Invoke();
    }

    private async Task DeleteCommentAsync()
    {
        if (!_ownedByUser) return;
        if (!_inEditMode) return;

        var result = await _deleteCommentDialog.OpenDialog();
        if (!result.WasConfirmed) return;
        
        _state = CompState.Deleted;
        StateHasChanged();
        await _commentRepo.DeleteAsync(Comment);
        
        OnCommentDeleted?.Invoke();
    }
    
    private bool UnsavedChanges()
    {
        return _tempContent != null && _tempContent != Comment.Content;
    }

    private string GetShownContent()
    {
        return (_inEditMode && _tempContent != null) ?
            _tempContent : Comment.Content;
    }

    private void NavigateToUser()
    {
        if (_poster == null) return;
        _navigationManager.NavigateTo(UserPage.GetRoute(_poster.Id));
    }
}