using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using MyMusicTaste.Database;
using MyMusicTaste.Database.Contexts.MongoDb;
using MyMusicTaste.Database.Operations;

namespace MyMusicTaste.Components.Forms;

public partial class RegisterUserForm : ComponentBase
{
    private class NewUserDto : IRegisterUserDto
    {
        [Required(ErrorMessage = "Enter your username.")]
        [StringLength(24)]
        public string? Username { get; set; }
        
        [Required(ErrorMessage = "Enter your email.")]
        public string? Email { get; set; }
        
        [Required(ErrorMessage = "Enter your password.")]
        [MinLength(8)]
        [MaxLength(32)]
        public string? Password { get; set; }
    }
    
    [SupplyParameterFromForm]
    private NewUserDto _newUser { get; set; } = new();
    
    private bool _submitted { get; set; } = false;
    private IEnumerable<string> _errors = new List<string>();
    
    private async Task SubmitAsync()
    {
        try
        {
            await Identity.RegisterUserAsync(_newUser);
            _submitted = true;
        }
        catch (UserRegistrationFailedException e)
        {
            _errors = e.Errors.Select(err => err.Description);
        }
    }
}