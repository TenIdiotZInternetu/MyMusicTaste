using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
using MyMusicTaste.Database;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.Comments;

/// <summary>
/// A component that displays a list of comments for a given page and allows signed-in users to post a new comment.
/// </summary>
public partial class CommentSection : ComponentBase
{
    /// <summary>
    /// The type of page the comments was posted to.
    /// </summary>
    [Parameter] public CommentPageType PageType { get; set; }
    
    /// <summary>
    /// The ID of the page for which to display comments.
    /// </summary>
    [Parameter] public string PageId { get; set; } = null!;
    
    /// <summary>
    /// The maximum number of comments to fetch and display.
    /// </summary>
    [Parameter] public int ResultsCount { get; set; }

    [Inject] private IIdentityProvider _identity { get; set; } = null!;
    [Inject] private ICommentsListing _commentsListing { get; set; } = null!;

    private enum ComponentState { Loading, Loaded}
    private ComponentState _state = ComponentState.Loading;
    
    private string? _signedUserId;
    private bool _userSigned => _signedUserId != null;
    
    private IEnumerable<Comment> _comments = null!;
    
    private Comment? _newComment;
    private CommentComp? _newCommentComp;
    private bool _newCommentShown;
    
    protected override async Task OnInitializedAsync()
    {
        _signedUserId = _identity.GetUserId();
        _comments = await _commentsListing.GetCommentsByPageAsync(PageType, PageId, ResultsCount);
        _state = ComponentState.Loaded;
    }

    private void ShowNewComment()
    {
        if (!_userSigned) return;
        
        _newComment = new Comment
        {
            UserId = new ObjectId(_signedUserId),
            CommentPageType = PageType,
            PageId = new ObjectId(PageId)
        };
        
        _newCommentShown = true;
        StateHasChanged();
    }
    
    // Hides the new comment, if it has been closed and not posted,
    // otherwise it stays shown, no more comments can be added until page reload
    // TODO: Create some smarter system that would allow adding more comments, and treat them as regular comments
    private void CloseNewComment()
    {
        if (_newCommentComp == null) return;
        _newCommentShown = _newCommentComp.IsPosted;
        StateHasChanged();
    }
}