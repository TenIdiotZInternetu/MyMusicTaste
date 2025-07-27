using Microsoft.AspNetCore.Components;
using MyMusicTaste.Components.Page_User;

namespace MyMusicTaste.Components.Page_Search;

public partial class UserSearchItem : ComponentBase
{
    [Parameter] public Models.User? User { get; set; }
    
    private void GoToUserPage()
    {
        Navigation.NavigateTo(UserPage.GetRoute(User!.Id));
    }
}