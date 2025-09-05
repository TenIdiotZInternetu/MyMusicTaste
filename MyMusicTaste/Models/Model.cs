using MongoDB.Bson;

namespace MyMusicTaste.Models;

/// <summary>
/// Base class for all entities in the system, providing a unique identifier.
/// </summary>
public abstract class Model
{
    public ObjectId Id { get; set; }
}