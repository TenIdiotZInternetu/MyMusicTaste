namespace MyMusicTaste.Models;

public class User : Model
{
    public required string Username { get; set; }
    
    public string? AboutMe { get; set; }
    public string? ProfilePictureLink { get; set; }
    public string? BannerPictureLink { get; set; }
}