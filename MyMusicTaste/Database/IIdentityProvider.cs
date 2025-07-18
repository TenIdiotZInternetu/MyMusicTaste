using Microsoft.AspNetCore.Identity;

namespace MyMusicTaste.Database;

public interface IIdentityProvider
{
    public Task<IdentityResult> SignUpUserAsync(IUserSignupDto userSignup);
    public Task<SignInResult> LoginUserAsync(IUserLoginDto userLoginDto);
    public Task LogOutUserAsync();
    public Task<IdentityResult> AssignRoleAsync(IdentityUser user, IdentityRole role);
}

public interface IUserSignupDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public interface IUserLoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}