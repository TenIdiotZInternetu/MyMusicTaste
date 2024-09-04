using MongoDB.Bson;

namespace MyMusicTaste.Database;

public interface IDbRepository<TModel>
{
    public TModel GetById(string? id);
    public TModel GetById(ObjectId id);
    public Task<TModel> GetByIdAsync(string? id);
    public void Create(TModel model);
    public Task CreateAsync(TModel model);
    public void Update(TModel model);
    public Task UpdateAsync(TModel model);
    public void Delete(TModel model);
    public Task DeleteAsync(TModel model);
}