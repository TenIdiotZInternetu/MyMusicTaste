namespace MyMusicTaste.Models;

/// <summary>
/// Represents statistical information about a user's ratings.
/// </summary>
public class UserStats
{
    // TODO: Replace strings by actual models
    
    /// <summary>
    /// Average ratings per album, as a list of (album name, mean rating) tuples.
    /// </summary>
    public List<(string, double)>? AlbumsByMean { get; set; }
    
    /// <summary>
    /// Average ratings per author, as a list of (author name, mean rating) tuples.
    /// </summary>
    public List<(string, double)>? AuthorsByMean { get; set; }
    
    /// <summary>
    /// Average ratings per genre, as a list of (genre name, mean rating) tuples.
    /// </summary>
    public List<(string, double)>? GenresByMean { get; set; }
}