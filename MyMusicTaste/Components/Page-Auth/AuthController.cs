using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MyMusicTaste.Components.Page_Home;
using MyMusicTaste.Database;

namespace MyMusicTaste.Components.Page_Auth;

[Route("auth")]
public class AuthController : Controller {
    public const string LOGIN_URI = "auth/login";
    public const string LOGOUT_URI = "auth/logout";
    
    public class LoginDto : IUserLoginDto
    {
        [Required(ErrorMessage = "Enter your username.")]
        [StringLength(24)]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Enter your password.")]
        [MinLength(8)]
        [MaxLength(32)]
        public string Password { get; set; } = null!;
    }
    
    private readonly IIdentityProvider _identity;

    public AuthController(IIdentityProvider identity) {
        _identity = identity;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromForm] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        var result = await _identity.LoginUserAsync(loginDto);

        if (result.Succeeded)
        {
            return Redirect(HomePage.GetRoute());
        }
        
        return Unauthorized();
    }
    
    
    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync() {
        await _identity.LogOutUserAsync();
        return Redirect("/");
    }
}