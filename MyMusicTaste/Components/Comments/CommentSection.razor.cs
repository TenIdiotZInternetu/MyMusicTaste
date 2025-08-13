using Microsoft.AspNetCore.Components;
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

    private enum ComponentState { NotLoaded, NotSignedIn, NotCommented, Commented}
    private ComponentState _state = ComponentState.NotLoaded;
    
    private string? _signedUserId;
    private Comment? _signedUserComment;

    private IEnumerable<Comment> _comments = null!;

    protected override async Task OnInitializedAsync()
    {
        _signedUserId = _identity.GetUserId();
        if (_signedUserId == null)
        {
            _state = ComponentState.NotSignedIn;
        }

        _comments = await _commentsListing.GetCommentsByPageAsync(PageType, PageId, ResultsCount);
        _signedUserComment = _comments.FirstOrDefault(c => c.UserId.ToString() == _signedUserId);
        _state = _signedUserComment == null ? 
            ComponentState.NotCommented : ComponentState.Commented;
    }
}