using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.Pages;

public partial class UserPage : ComponentBase
{
    public const string RouteTemplate = "/users/{UserId}";
    
    [Parameter]
    public string? UserId { get; set; }
    
    private User? _user { get; set; }
    private IEnumerable<SongRating> _ratings { get; set; }
    
    private enum PageState { Loading, Loaded, UserNotFound }
    private PageState _pageState { get; set; } = PageState.Loading;

    private string _aboutMeText => _user?.AboutMe ?? "I'm a mysterious person.";
    
    public static string GetRoute(ObjectId userId)
    {
        return RouteTemplate.Replace("{UserId}", userId.ToString());
    }
    
    protected override void OnInitialized()
    {
        try
        {
            _user = UserRepository.GetById(UserId);
            _ratings = RatingListing.GetRatingsByUser(_user);
            _pageState = PageState.Loaded;
        }
        catch (EntryNotFoundException e)
        {
            _pageState = PageState.UserNotFound;
        }
    }
}