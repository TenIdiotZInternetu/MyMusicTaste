using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;

namespace MyMusicTaste.Database.Contexts.MongoDb;

[CollectionName("Users")]
public class MongoUser : MongoIdentityUser<ObjectId> {}

[CollectionName("Roles")]
public class MongoRole : MongoIdentityRole<ObjectId> {}

public class MongoIdentity
{
    
}