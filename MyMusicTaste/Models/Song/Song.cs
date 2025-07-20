namespace MyMusicTaste.Models;

public class Song : Model
{
    private static readonly DateOnly INVALID_DATE = DateOnly.MaxValue;

    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string? Album { get; set; }
    public string? Genre { get; set; }
    
    public DateOnly ReleaseDate { get; set; } = INVALID_DATE;
    public bool ReleasDateValid => ReleaseDate != INVALID_DATE;
    public string? ReleaseDateString => ReleasDateValid ? 
        null : ReleaseDate.ToString();
    
    public string? CoverImageLink { get; set; }
    public string? SourceLink { get; private set; }

    public bool ValidateSourceLink(string link)
    {
        string[] validDomains =
        {
            "youtube.com/watch", 
            "music.youtube.com",
            "youtu.be/",
            "open.spotify.com/track",
            "soundcloud.com/",
            "music.apple.com/"
        };

        if (!validDomains.Any(link.Contains))
        {
            return false;
        }
        
        SourceLink = link;
        return true;
    }
}