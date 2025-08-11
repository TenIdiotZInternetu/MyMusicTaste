namespace MyMusicTaste.Models;

public class SongStats
{
    public int TotalListens { get; set; }
    public float AverageRating { get; set; }
    public float MedianRating { get; set; }
    public int[] RatingDistribution { get; set; } = new int[20];
    
    public bool NoData => TotalListens <= 0;

    public static int[] CreateDistributionBoundaries()
    {
        int[] boundaries = new int[21];
        for (int i = 0; i < boundaries.Length; i++)
        {
            boundaries[i] = i * 5 + 1;
        }
        return boundaries;
    }
}