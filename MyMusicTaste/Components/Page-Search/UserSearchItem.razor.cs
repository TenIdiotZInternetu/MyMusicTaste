using Microsoft.AspNetCore.Components;
using MyMusicTaste.Components.Page_User;

namespace MyMusicTaste.Components.Page_Search;

/// <summary>
/// Displays an item in search results that navigates to the user's page when clicked.
/// </summary>
public partial class UserSearchItem : ComponentBase
{
    /// <summary>
    /// The user data to display.
    /// </summary>
    [Parameter] public Models.User? User { get; set; }
    
    private void GoToUserPage()
    {
        Navigation.NavigateTo(UserPage.GetRoute(User!.Id));
    }
}