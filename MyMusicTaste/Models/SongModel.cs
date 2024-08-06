using System.ComponentModel.DataAnnotations;

namespace MyMusicTaste.Models;

public class SongModel
{
    public long Id { get; init; } = _nextId++;
    
    [Required]
    [StringLength(256)]
    public string Title { get; set; }
    
    [Required]
    [StringLength(256)]
    public string Artist { get; set; }
    
    public string Album { get; set; }
    
    public string Genre { get; set; }
    
    public DateOnly ReleaseDate { get; set; }
    
    private static long _nextId = 1;
}