using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyMusicTaste.Database.Contexts.MongoDb;

public class MongoDto<TModel>(TModel model)
{
    [BsonElement("_id")] 
    public ObjectId Id;
    
    public TModel? Model { get; set; } = model;
    public bool ModelIsValid => Model != null;
}