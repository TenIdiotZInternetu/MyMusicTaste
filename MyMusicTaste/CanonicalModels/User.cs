using MyMusicTaste.CanonicalModels;

namespace MyMusicTaste.Models;

public class User : Model
{
    public string Username { get; set; }
    
    public string? AboutMe { get; set; }
    public string? ProfilePictureLink { get; set; }
    public string? BannerPictureLink { get; set; }
    
    public List<SongRating> SongRatings { get; set; } = new();

    public User(string username)
    {
        Username = username;
    }
}