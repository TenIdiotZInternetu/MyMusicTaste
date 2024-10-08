namespace MyMusicTaste.Models;

public class Song : Model
{
    private static readonly DateOnly InvalidDate = DateOnly.MaxValue;

    public string? Title { get; set; }
    public string? Author { get; set; }

    public string Album { get; set; } = "Unknown";
    public string Genre { get; set; } = "Unknown";
    public DateOnly ReleaseDate { get; set; } = InvalidDate;
    
    public string? CoverImageLink { get; set; }
    
    public string ReleaseDateString => ReleaseDate == InvalidDate ? 
        "Unknown" : ReleaseDate.ToString();
}