using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using MyMusicTaste.Database;

namespace MyMusicTaste.Components.Page_Auth;

public partial class UserSignupForm : ComponentBase
{
    private class _NewUserSignupDto : IUserSignupDto
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
    private _NewUserSignupDto NewUserSignup { get; set; } = new();
    
    private bool _submitted { get; set; }
    private IEnumerable<string> _errors = new List<string>();
    
    private async Task SubmitAsync()
    {
        var result = await Identity.SignUpUserAsync(NewUserSignup);

        if (!result.Succeeded)
        {
            _errors = result.Errors.Select(err => err.Description);
            return;
        }
        
        _submitted = true;
    }
}