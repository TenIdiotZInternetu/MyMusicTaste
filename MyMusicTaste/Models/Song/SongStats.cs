namespace MyMusicTaste.Models;

public class SongStats
{
    public int TotalListens { get; set; }
    public float AverageRating { get; set; }
    public float MedianRating { get; set; }
    public int[] RatingDistribution { get; set; } = new int[20];
    
    public bool NoData => TotalListens <= 0;
}