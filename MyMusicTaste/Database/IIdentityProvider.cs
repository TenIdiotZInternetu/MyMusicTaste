namespace MyMusicTaste.Database;

public interface IIdentityProvider
{
    public Task SignUpUserAsync(IUserSignupDto userSignup);
    // public Task AssignRoleAsync(TUser user, TRole role);
}

public interface IUserSignupDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
