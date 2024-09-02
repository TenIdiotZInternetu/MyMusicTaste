using MongoDB.Bson;
using MongoDB.Driver;
using MyMusicTaste.Database.Connections;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb;

public class MongoRepository<TModel> : IDbRepository<TModel>
    where TModel : Model
{
    public IMongoCollection<TModel> Collection { get; } = MongoCollectionFactory.Create<TModel>();
    public IMongoDatabase Database => Collection.Database;

    public TModel GetById(string? id)
    {
        bool idIsValid = ObjectId.TryParse(id, out ObjectId guid);
        
        if (!idIsValid)
        {
            throw new EntryNotFoundException("Invalid ID!");
        }

        return GetById(guid);
    }
    
    public TModel GetById(ObjectId id)
    {
        var filter = Builders<TModel>.Filter
            .Eq(x => x.Id, id);

        TModel model = Collection.Find(filter).FirstOrDefault();
        
        if (model == null)
        {
            throw new EntryNotFoundException("Entry not found!");
        }

        model.Id = id;
        return model;
    }

    public Task<TModel> GetByIdAsync(string? id)
    {
        throw new NotImplementedException();
    }

    public void Create(TModel model)
    {
        throw new NotImplementedException();
    }

    public Task CreateAsync(TModel model)
    {
        return Collection.InsertOneAsync(model);
    }

    public void Update(TModel model)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(TModel model)
    {
        throw new NotImplementedException();
    }

    public void Delete(TModel model)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(TModel model)
    {
        throw new NotImplementedException();
    }
}