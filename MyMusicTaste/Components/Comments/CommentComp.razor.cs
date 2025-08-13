using Microsoft.AspNetCore.Components;
using MyMusicTaste.Database;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.Comments;

public partial class CommentComp() : ComponentBase
{
    [Parameter] public Comment? Comment { get; set; }

    [Inject] private IDbRepository<User> _userRepo { get; set; } = null!;
    
    private User? _user;

    protected override void OnInitialized()
    {
        if (Comment == null) return;
        _user = _userRepo.GetById(Comment.UserId);
    }
}