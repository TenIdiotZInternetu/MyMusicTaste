using MongoDB.Bson;
using MongoDB.Driver;
using MyMusicTaste.Database.Models;

namespace MyMusicTaste.Database.Operations;

public static class GenericDbOperations
{
    public static TModel GetModelById<TModel>(string? id)
        where TModel : IMongoDbModel<TModel>
    {
        bool idIsValid = ObjectId.TryParse(id, out ObjectId objectId);
        
        if (!idIsValid)
        {
            throw new EntryNotFoundException("Invalid ID!");
        }

        return GetModelById<TModel>(objectId);
    }
    
    public static TModel GetModelById<TModel>(ObjectId id)
        where TModel : IMongoDbModel<TModel>
    {
        var collection = TModel.Collection;
        var filter = Builders<TModel>.Filter
            .Eq(x => x.Id, id);

        TModel entry = collection.Find(filter).FirstOrDefault();
        
        if (entry == null)
        {
            throw new EntryNotFoundException("Entry not found!");
        }

        return entry;
    }
}