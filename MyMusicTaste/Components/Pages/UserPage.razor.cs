using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
using MongoDB.Driver;
using MyMusicTaste.Database.Operations;

namespace MyMusicTaste.Components.Pages;

public partial class UserPage : ComponentBase
{
    private enum PageState { Loading, Loaded, UserNotFound }
    
    [Parameter]
    public string? UserId { get; set; }
    
    private Models.User? _user { get; set; }
    private PageState _pageState { get; set; } = PageState.Loading;
    
    protected override void OnInitialized()
    {
        try
        {
            _user = UserRepository.GetById(UserId);
            _pageState = PageState.Loaded;
        }
        catch (EntryNotFoundException e)
        {
            _pageState = PageState.UserNotFound;
        }
    }
}