using Microsoft.AspNetCore.Components;
using MyMusicTaste.Components.Pages;
using MyMusicTaste.Models;

namespace MyMusicTaste.Components.SearchComps;

public partial class UserSearchItem : ComponentBase
{
    [Parameter] 
    public User? User { get; set; }
    
    private void GoToUserPage()
    {
        Navigation.NavigateTo(UserPage.GetRoute(User!.Id));
    }
}