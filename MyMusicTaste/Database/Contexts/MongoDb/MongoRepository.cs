using MongoDB.Bson;
using MongoDB.Driver;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb;

public class MongoRepository<TModel> : IDbRepository<TModel>
    where TModel : Model
{
    public IMongoCollection<TModel> Collection { get; } = MongoCollectionFactory.Create<TModel>();

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

    public async Task<TModel> GetByIdAsync(string? id)
    {
        var objectId = new ObjectId(id);
        
        var filter = Builders<TModel>.Filter
            .Eq(x => x.Id, objectId);

        TModel model = await Collection.Find(filter).FirstOrDefaultAsync();
        
        if (model == null)
        {
            throw new EntryNotFoundException("Entry not found!");
        }

        model.Id = objectId;
        return model;
    }

    public async Task<IEnumerable<TModel>> GetByIdsAsync(IEnumerable<string> ids)
    {
        var filter = Builders<TModel>.Filter.In(x => x.Id, ids.Select(ObjectId.Parse));
        return await Collection.Find(filter).ToListAsync();
    }

    public Task CreateAsync(TModel model)
    {
        return Collection.InsertOneAsync(model);
    }

    public async Task UpdateAsync(TModel model)
    {
        await Collection.ReplaceOneAsync(doc => doc.Id == model.Id, model);
    }

    public async Task DeleteAsync(TModel model)
    {
        await Collection.DeleteOneAsync(doc => doc.Id == model.Id);
    }
}