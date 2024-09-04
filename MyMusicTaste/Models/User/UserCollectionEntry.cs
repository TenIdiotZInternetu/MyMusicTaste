namespace MyMusicTaste.Models;

public class UserCollectionEntry : Model
{
    public User? User { get; set; }
    public Song? Song { get; set; }
    public int Rating { get; set; }
}