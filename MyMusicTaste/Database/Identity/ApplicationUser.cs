using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;

namespace MyMusicTaste.Database.Identity;

[CollectionName("Users")]
public class ApplicationUser : MongoIdentityUser<ObjectId>
{
    
}