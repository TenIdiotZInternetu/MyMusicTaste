using System.ComponentModel.DataAnnotations;

namespace MyMusicTaste.Database.Models;

public class UserModel
{
    [Required]
    [StringLength(24)]
    public string Username { get; set; }
}