using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using MyMusicTaste.Components.Page_Home;
using MyMusicTaste.Database;

namespace MyMusicTaste.Components.Page_Auth;

public partial class UserSignupForm : ComponentBase
{
    [Inject] private NavigationManager _navigation { get; set; } = null!;
    [Inject] private IIdentityProvider _identity { get; set; } = null!;
    
    private class _NewUserSignupDto : IUserSignupDto, IUserLoginDto
    {
        [Required(ErrorMessage = "Enter your username.")]
        [StringLength(24)]
        public string Username { get; set; } = null!;
        
        [Required(ErrorMessage = "Enter your email.")]
        public string Email { get; set; } = null!;
        
        [Required(ErrorMessage = "Enter your password.")]
        public string Password { get; set; } = null!;
    }
    
    [SupplyParameterFromForm(FormName = "UserSignup")]
    private _NewUserSignupDto _newUserSignup { get; set; } = new();
    
    private bool _submitted { get; set; }
    private IEnumerable<string> _errors = new List<string>();
    
    private async Task SubmitAsync()
    {
        var result = await _identity.SignUpUserAsync(_newUserSignup);

        if (!result.Succeeded)
        {
            _errors = result.Errors.Select(err => err.Description);
            StateHasChanged();
            return;
        }

        _submitted = true;
        StateHasChanged();
        
        await _identity.LoginUserAsync(_newUserSignup);
        _navigation.NavigateTo(HomePage.GetRoute(), true);
    }
}