using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb;

public interface IMongoDto
{
    public ObjectId Id { get; init; }
}

public class MongoSongDto : Song, IMongoDto
{
    [BsonElement("_id")] 
    public ObjectId Id { get; init; }
}

public class MongoUserDto : User, IMongoDto
{
    [BsonElement("_id")] 
    public ObjectId Id { get; init; }
}