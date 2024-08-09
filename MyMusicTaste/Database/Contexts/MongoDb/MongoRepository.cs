using MongoDB.Bson;
using MongoDB.Driver;
using MyMusicTaste.Database.Connections;
using MyMusicTaste.Database.Operations;

namespace MyMusicTaste.Database.Contexts.MongoDb;

public class MongoRepository<TModel> : IDbRepository<TModel>
{
    public IMongoCollection<TModel> Collection { get; init; }
    public IMongoDatabase Database => Collection.Database;

    public MongoRepository(string databaseName, string collectionName)
    {
        var client = MongoDbContext.Client;
        Collection = client.GetDatabase(databaseName).GetCollection<TModel>(collectionName);
    }
    
    public TModel GetById(string? id)
    {
        bool idIsValid = ObjectId.TryParse(id, out ObjectId objectId);
        
        if (!idIsValid)
        {
            throw new EntryNotFoundException("Invalid ID!");
        }

        return GetById(objectId);
    }
    
    public TModel GetById(ObjectId id)
    {
        var filter = Builders<TModel>.Filter
            .Eq(x => x.Id, id);

        TModel entry = Collection.Find(filter).FirstOrDefault();
        
        if (entry == null)
        {
            throw new EntryNotFoundException("Entry not found!");
        }

        return entry;
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
        throw new NotImplementedException();
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