namespace MyMusicTaste.Database;

public interface IIdentityProvider<TUser, TRole>
{
    public Task RegisterUserAsync(TUser user);
    public Task AssignRoleAsync(TUser user, TRole role);
}