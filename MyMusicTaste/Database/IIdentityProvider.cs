namespace MyMusicTaste.Database;

public interface IIdentityProvider<TUser, TRole>
{
    public void AddIdentity(WebApplicationBuilder builder);
    public void RegisterUser(TUser user);
    public void AssignRole(TUser user, TRole role);
}