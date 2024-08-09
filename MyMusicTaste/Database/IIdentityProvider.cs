namespace MyMusicTaste.Database;

public interface IIdentityProvider<TUser, TRole>
{
    public void AddIdentity();
    public void RegisterUser(TUser user);
    public void AssignRole(TUser user, TRole role);
}