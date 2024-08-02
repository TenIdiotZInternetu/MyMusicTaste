namespace MyMusicTaste.Models;

public record struct Song(
    long Id,
    string Title,
    string Artist,
    string Album,
    string Genre,
    DateOnly ReleaseDate
);