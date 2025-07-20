using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
using MyMusicTaste.Database;

namespace MyMusicTaste.Models;

public class SongRating : Model
{
    private const byte NOT_RATED = 255;
    
    public ObjectId UserId { get; set; }
    public ObjectId SongId { get; set; }
    public byte Rating { get; set; } = NOT_RATED;

    public bool IsValid => UserId != default && SongId != default;
}