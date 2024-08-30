using MongoDB.Bson;

namespace MyMusicTaste.Models;

public abstract class Model
{
    public ObjectId Id { get; set; }
}