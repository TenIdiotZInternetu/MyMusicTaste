namespace MyMusicTaste.Models;

public class UserStats
{
    // TODO: Replace strings by actual models
    public List<(string, double)>? AlbumsByMean { get; set; }
    public List<(string, double)>? AuthorsByMean { get; set; }
    public List<(string, double)>? GenresByMean { get; set; }
}