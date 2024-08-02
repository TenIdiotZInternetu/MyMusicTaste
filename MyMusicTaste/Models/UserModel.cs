namespace MyMusicTaste.Models;

public class UserModel
{
    public long Id { get; init; } = _nextId++;
    
    public string Username { get; set; }
    
    private static long _nextId = 1;
}