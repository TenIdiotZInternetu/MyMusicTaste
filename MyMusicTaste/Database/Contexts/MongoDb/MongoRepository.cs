using MongoDB.Bson;
using MongoDB.Driver;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb;

/// <summary>
/// A MongoDB-based repository for performing CRUD operations on models.
/// </summary>
/// <typeparam name="TModel">The type of model managed by this repository. Must inherit from <see cref="Model"/>.</typeparam>
public class MongoRepository<TModel> : IDbRepository<TModel>
    where TModel : Model
{
    /// <summary>
    /// The MongoDB collection associated with the model type.
    /// </summary>
    public IMongoCollection<TModel> Collection { get; } = MongoCollectionFactory.Create<TModel>();

    /// <summary>
    /// Retrieves a model by its ID.
    /// </summary>
    /// <param name="id">The string representation of the model's ObjectId.</param>
    /// <returns>The model corresponding to the specified ID.</returns>
    /// <exception cref="EntryNotFoundException">Thrown when the ID is invalid or no entry is found.</exception>
    public TModel GetById(string? id)
    {
        bool idIsValid = ObjectId.TryParse(id, out ObjectId guid);
        
        if (!idIsValid)
        {
            throw new EntryNotFoundException("Invalid ID!");
        }

        return GetById(guid);
    }
    
    /// <summary>
    /// Retrieves a model by its MongoDB <see cref="ObjectId"/>.
    /// </summary>
    /// <param name="id">The MongoDB ObjectId of the model.</param>
    /// <returns>The model corresponding to the specified ID.</returns>
    /// <exception cref="EntryNotFoundException">Thrown when no entry is found.</exception>
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

    /// <summary>
    /// Asynchronously retrieves a model by its ID.
    /// </summary>
    /// <param name="id">The string representation of the model's ObjectId.</param>
    /// <returns>A task for the model corresponding to the specified ID.</returns>
    /// <exception cref="EntryNotFoundException">Thrown when no entry is found.</exception>
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

    /// <summary>
    /// Asynchronously retrieves multiple models by their IDs.
    /// </summary>
    /// <param name="ids">The collection of string IDs to retrieve.</param>
    /// <returns>A task for the collection of matching models.</returns>
    public async Task<IEnumerable<TModel>> GetByIdsAsync(IEnumerable<string> ids)
    {
        var filter = Builders<TModel>.Filter.In(x => x.Id, ids.Select(ObjectId.Parse));
        return await Collection.Find(filter).ToListAsync();
    }

    /// <summary>
    /// Asynchronously inserts a new model into the database collection.
    /// </summary>
    /// <param name="model">The model to insert.</param>
    public async Task CreateAsync(TModel model)
    {
        await Collection.InsertOneAsync(model);
    }

    /// <summary>
    /// Asynchronously updates an existing model in the database collection.
    /// </summary>
    /// <param name="model">The model with updated values.</param>
    public async Task UpdateAsync(TModel model)
    {
        await Collection.ReplaceOneAsync(doc => doc.Id == model.Id, model);
    }

    /// <summary>
    /// Asynchronously deletes a model from the collection.
    /// </summary>
    /// <param name="model">The model to delete.</param>
    public async Task DeleteAsync(TModel model)
    {
        await Collection.DeleteOneAsync(doc => doc.Id == model.Id);
    }
}