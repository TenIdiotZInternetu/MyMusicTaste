namespace MyMusicTaste.Database.Contexts.MongoDb;

public class MongoRepository<TModel> : IDbRepository<TModel>
{
    public TModel GetById(string? id)
    {
        throw new NotImplementedException();
    }

    public void Create(TModel model)
    {
        throw new NotImplementedException();
    }

    public void Update(TModel model)
    {
        throw new NotImplementedException();
    }

    public void Delete(TModel model)
    {
        throw new NotImplementedException();
    }
}