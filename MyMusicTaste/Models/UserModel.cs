using System.ComponentModel.DataAnnotations;

namespace MyMusicTaste.Models;

public class UserModel
{
    public long Id { get; init; } = _nextId++;
    
    [Required]
    [StringLength(24)]
    public string Username { get; set; }
    
    private static long _nextId = 1;
}