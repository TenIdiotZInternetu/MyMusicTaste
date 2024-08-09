using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;

namespace MyMusicTaste.Database.Identity;

public class ApplicationRole : MongoIdentityRole<ObjectId>
{
    
}