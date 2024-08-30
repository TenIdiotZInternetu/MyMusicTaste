using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;

namespace MyMusicTaste.Database.Contexts.MongoDb;

[CollectionName("Users")]
public class MongoUser : MongoIdentityUser<ObjectId> {}

[CollectionName("Roles")]
public class MongoRole : MongoIdentityRole<ObjectId> {}

public class MongoIdentity : IIdentityProvider<MongoUser, MongoRole>
{
    private readonly UserManager<MongoUser> _userManager;

    public MongoIdentity(UserManager<MongoUser> userManager)
    {
        _userManager = userManager;
    }

    public static void Configure(WebApplicationBuilder builder)
    {
        var dbConnectionString = builder.Configuration["MONGO_URI"];

        builder.Services.AddIdentity<MongoUser, MongoRole>()
            .AddMongoDbStores<MongoUser, MongoRole, ObjectId>(dbConnectionString, "Security")
            .AddDefaultTokenProviders();
        
        builder.Services.AddScoped<IIdentityProvider<MongoUser, MongoRole>, MongoIdentity>();
    }

    public async Task RegisterUserAsync(MongoUser user)
    {
        var result = await _userManager.CreateAsync(user);
        
        if (!result.Succeeded)
        {
            throw new AuthenticationFailureException("Failed to register user.");
        }
    }

    public async Task AssignRoleAsync(MongoUser user, MongoRole role)
    {
        throw new NotImplementedException();
    }
}