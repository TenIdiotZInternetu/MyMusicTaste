using MongoDB.Bson;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database;

/// <summary>
/// Represents a generic repository for performing CRUD operations on models.
/// </summary>
/// <typeparam name="TModel">The type of model managed by this repository. Must inherit from <see cref="Model"/>.</typeparam>
public interface IDbRepository<TModel>
{
    /// <summary>
    /// Retrieves a model by its ID from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the model.</param>
    /// <returns>The model corresponding to the specified ID.</returns>
    /// <exception cref="EntryNotFoundException">Thrown when the entry does not exist.</exception>
    public TModel GetById(string? id);
    
    /// <summary>
    /// Retrieves a model by its ID from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the model.</param>
    /// <returns>The model corresponding to the specified ID.</returns>
    /// <exception cref="EntryNotFoundException">Thrown when the entry does not exist.</exception>
    [Obsolete("Use a Database-agnostic overload GetById(string?) instead.")]
    public TModel GetById(ObjectId id); // TODO: Remove this
    
    /// <summary>
    /// Retrieves a model by its string ID from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the model.</param>
    /// <returns>A task for the retrieved model.</returns>
    /// <exception cref="EntryNotFoundException">Thrown when the entry does not exist.</exception>
    public Task<TModel> GetByIdAsync(string? id);   // TODO: why is this nullable?
    
    /// <summary>
    /// Asynchronously retrieves multiple models by their IDs form the database.
    /// </summary>
    /// <param name="ids">The collection of unique identifiers to retrieve.</param>
    /// <returns>A task for the collection of retrieved models.</returns>
    public Task<IEnumerable<TModel>> GetByIdsAsync(IEnumerable<string> ids);
    
    /// <summary>
    /// Asynchronously inserts a new model to the database.
    /// </summary>
    /// <param name="model">The model to insert.</param>
    public Task CreateAsync(TModel model);
    
    /// <summary>
    /// Asynchronously updates an existing model in the database.
    /// </summary>
    /// <param name="model">The model with updated values.</param>
    public Task UpdateAsync(TModel model);
    
    /// <summary>
    /// Asynchronously deletes a model from the database.
    /// </summary>
    /// <param name="model">The model to delete.</param>
    public Task DeleteAsync(TModel model);
    
    // TODO: Find by model field values
}