namespace MyMusicTaste.Database;

public interface IDbRepository<TModel>
{
    public TModel GetById(string? id);
    public void Create(TModel model);
    public void Update(TModel model);
    public void Delete(TModel model);
}