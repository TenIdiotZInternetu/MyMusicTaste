using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
using MyMusicTaste.Database;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.Comments;

public partial class CommentSection : ComponentBase
{
    [Parameter] public CommentPageType PageType { get; set; }
    [Parameter] public string PageId { get; set; } = null!;
    [Parameter] public int ResultsCount { get; set; }

    [Inject] private IIdentityProvider _identity { get; set; } = null!;
    [Inject] private ICommentsListing _commentsListing { get; set; } = null!;
    [Inject] private IDbRepository<User> _userRepository { get; set; } = null!;

    private enum ComponentState { Loading, Loaded}
    private ComponentState _state = ComponentState.Loading;
    
    private string? _signedUserId;
    private bool _userSigned => _signedUserId != null;
    
    private IEnumerable<Comment> _comments = null!;
    
    private Comment? _newComment;
    private CommentComp _newCommentComp;
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
        _newCommentShown = !_newCommentComp.IsPosted;
        StateHasChanged();
    }
}