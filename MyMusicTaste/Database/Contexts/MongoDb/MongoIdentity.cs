using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;

namespace MyMusicTaste.Database.Contexts.MongoDb;

[CollectionName("Users")]
public class MongoUser : MongoIdentityUser<ObjectId> {}

[CollectionName("Roles")]
public class MongoRole : MongoIdentityRole<ObjectId> {}

public class MongoIdentity : IIdentityProvider<MongoUser, MongoRole>
{
    public void AddIdentity(WebApplicationBuilder builder)
    {
        var dbConnectionString = builder.Configuration["MONGO_URI"];
        
        builder.Services.AddIdentity<MongoUser, MongoRole>()
            .AddMongoDbStores<MongoUser, MongoRole, ObjectId>(dbConnectionString, "Security");
    }

    public void RegisterUser(MongoUser user)
    {
        throw new NotImplementedException();
    }

    public void AssignRole(MongoUser user, MongoRole role)
    {
        throw new NotImplementedException();
    }
}