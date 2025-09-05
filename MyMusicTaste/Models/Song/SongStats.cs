namespace MyMusicTaste.Models;

/// <summary>
/// Represents statistical information about a song, including ratings and listens.
/// </summary>
public class SongStats
{
    /// <summary>
    /// Total number of times the song has been rated.
    /// </summary>
    public int TotalListens { get; set; }
    
    public float AverageRating { get; set; }
    public float MedianRating { get; set; }
    
    /// <summary>
    /// Distribution of ratings across 20 buckets, grouped by 1-5, 6-10, etc.
    /// </summary>
    public int[] RatingDistribution { get; set; } = new int[20];
    
    /// <summary>
    /// True if no ratings of the song exist.
    /// </summary>
    public bool NoData => TotalListens <= 0;
}