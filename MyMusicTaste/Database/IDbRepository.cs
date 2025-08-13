using MongoDB.Bson;

namespace MyMusicTaste.Database;

public interface IDbRepository<TModel>
{
    public TModel GetById(string? id);
    public TModel GetById(ObjectId id);
    public Task<TModel> GetByIdAsync(string? id);
    public Task<IEnumerable<TModel>> GetByIdsAsync(IEnumerable<string> ids);
    public Task CreateAsync(TModel model);
    public Task UpdateAsync(TModel model);
    public Task DeleteAsync(TModel model);
}