namespace MyMusicTaste.Models;

public class UserSongListEntry : Model
{
    public User? User { get; set; }
    public Song? Song { get; set; }
    public int Rating { get; set; }
}