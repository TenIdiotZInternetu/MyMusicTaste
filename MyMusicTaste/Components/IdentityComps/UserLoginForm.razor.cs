using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using MyMusicTaste.Components.Pages;
using MyMusicTaste.Database;
using MyMusicTaste.Database.Operations;

namespace MyMusicTaste.Components.IdentityComps;

public partial class UserLoginForm : ComponentBase
{
    private class _LoginDto : IUserLoginDto
    {
        [Required(ErrorMessage = "Enter your username.")]
        [StringLength(24)]
        public string? Username { get; set; }
        
        [Required(ErrorMessage = "Enter your password.")]
        [MinLength(8)]
        [MaxLength(32)]
        public string? Password { get; set; }
    }
    
    [SupplyParameterFromForm(FormName = "UserLogin")]
    private _LoginDto LoginDto { get; set; } = new();

    [Inject] 
    private NavigationManager _navigation { get; set; }

    private bool _invalidCredentials { get; set; } = false;
    
    private async Task SubmitAsync()
    {
        var result = await Identity.LoginUserAsync(LoginDto);

        if (result.IsNotAllowed)
        {
            _invalidCredentials = true;
        }

        if (result.Succeeded)
        {
            _navigation.NavigateTo(HomePage.GetRoute());
        }
    }
}