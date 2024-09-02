using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
using MyMusicTaste.Database.Operations;

namespace MyMusicTaste.Components.Pages;

public partial class UserPage : ComponentBase
{
    public const string RouteTemplate = "/users/{UserId}";
    
    [Parameter]
    public string? UserId { get; set; }
    
    private enum PageState { Loading, Loaded, UserNotFound }
    private Models.User? _user { get; set; }
    private PageState _pageState { get; set; } = PageState.Loading;

    public static string GetRoute(ObjectId userId)
    {
        return RouteTemplate.Replace("{UserId}", userId.ToString());
    }
    
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