using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.Page_User;

public partial class UserPage : ComponentBase
{
    public const string ROUTE_TEMPLATE = "/users/{UserId}";
    
    [Parameter] public string? UserId { get; set; }
    
    private User? _user { get; set; }
    private IEnumerable<SongRating>? _ratings { get; set; }
    
    private enum PageState { Loading, Loaded, UserNotFound }
    private PageState _pageState { get; set; } = PageState.Loading;

    private string _aboutMeText => _user?.AboutMe ?? "I'm a mysterious person.";
    
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
}