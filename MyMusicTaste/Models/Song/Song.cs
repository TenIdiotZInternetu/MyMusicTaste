namespace MyMusicTaste.Models;

public class Song : Model
{
    private static readonly DateOnly INVALID_DATE = DateOnly.MaxValue;

    public string? Title { get; set; }
    public string? Author { get; set; }

    public string Album { get; set; } = "Unknown";
    public string Genre { get; set; } = "Unknown";
    public DateOnly ReleaseDate { get; set; } = INVALID_DATE;
    
    public string? CoverImageLink { get; set; }
    
    public string ReleaseDateString => ReleaseDate == INVALID_DATE ? 
        "Unknown" : ReleaseDate.ToString();
}