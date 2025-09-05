using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MyMusicTaste.Components.Page_Home;
using MyMusicTaste.Database;

namespace MyMusicTaste.Components.Page_Auth;

/// <summary>
/// Controller responsible for authentication operations.
/// </summary>
[Route("auth")]
public class AuthController : Controller {
    public const string LOGIN_ROUTE = "auth/login";
    public const string LOGOUT_ROUTE = "auth/logout";
    
    /// <summary>
    /// DTO used for login requests.
    /// </summary>
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

    /// <summary>
    /// Logs in a user using provided credentials.
    /// Returns a redirect to the home page on success, or Unauthorized/BadRequest on failure.
    /// </summary>
    /// <param name="loginDto">Login credentials.</param>
    /// <returns>An action result representing success or failure of the login attempt.</returns>
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
    
    /// <summary>
    /// Logs out the currently authenticated user and redirects to the home page.
    /// </summary>
    /// <returns>An action result redirecting to the homepage.</returns>
    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync() {
        await _identity.LogOutUserAsync();
        return Redirect(HomePage.GetRoute());
    }
}