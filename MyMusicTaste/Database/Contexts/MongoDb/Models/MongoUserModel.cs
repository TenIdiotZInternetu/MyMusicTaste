using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Models;

public class UserSongModel : User
{
    public UserSongModel(string username) : base(username)
    {
        
    }
}