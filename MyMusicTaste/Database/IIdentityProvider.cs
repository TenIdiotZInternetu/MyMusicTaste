namespace MyMusicTaste.Database;

public interface IIdentityProvider
{
    public Task RegisterUserAsync(IRegisterUserDto user);
    // public Task AssignRoleAsync(TUser user, TRole role);
}

public interface IRegisterUserDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
