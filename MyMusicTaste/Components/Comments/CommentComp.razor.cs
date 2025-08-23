using Microsoft.AspNetCore.Components;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MyMusicTaste.Components.Dialogs;
using MyMusicTaste.Database;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.Comments;

public partial class CommentComp() : ComponentBase
{
    [Parameter] public Comment Comment { get; set; } = null!;
    [Parameter] public bool IsUnposted { get; set; }

    [Inject] private IDbRepository<User> _userRepo { get; set; } = null!;
    [Inject] private IDbRepository<Comment> _commentRepo { get; set; } = null!;
    [Inject] private IIdentityProvider _identity { get; set; } = null!;
    
    private User? _poster;
    private bool _ownedByUser;
    
    private bool _inEditMode;
    private string? _tempContent;

    private string _content => GetShownContent();
    private bool _showLastEdit => Comment.LastEditTimeValid;
    private string _saveBtnStyle => UnsavedChanges() ? "primary" : "secondary";
    
    private ConfirmDialog _unsavedChangesDialog = null!;
    
    protected override void OnInitialized()
    {
        _poster = _userRepo.GetById(Comment.UserId);
        _ownedByUser = _identity.AuthorizeUserById(_poster.Id.ToString());
        _inEditMode = IsUnposted;
    }

    private void OpenEditMode()
    {
        if (!_ownedByUser) return;
        _inEditMode = true;
        StateHasChanged();
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
            await _commentRepo.CreateAsync(Comment);
        }
        else
        {
            Comment.LastEditTime = DateTime.Now;
            await _commentRepo.UpdateAsync(Comment);
        }
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
}