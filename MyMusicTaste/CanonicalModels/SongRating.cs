using MyMusicTaste.Models;

namespace MyMusicTaste.CanonicalModels;

public class SongRating : Model
{
    public User? User { get; set; }
    public Song? Song { get; set; }
    public int Rating { get; set; }
}