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
    
    public string? CoverImageLink { get; set; }
    public string? SourceLink { get; set; }
}