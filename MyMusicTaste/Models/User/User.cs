namespace MyMusicTaste.Models;

public class User : Model
{
    public string Username { get; set; }
    
    public string? AboutMe { get; set; }
    public string? ProfilePictureLink { get; set; }
    public string? BannerPictureLink { get; set; }
    
    public UserStats? Stats { get; set; }
    public List<UserSongListEntry> List { get; set; } = new();
}