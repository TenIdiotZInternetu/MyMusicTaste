using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace MyMusicTaste.Database.Models;

public interface IMongoDbModel<TModel>
{
    public static abstract IMongoCollection<TModel> Collection { get; }
    
    [BsonElement("_id")]
    public ObjectId Id { get; set; }
}