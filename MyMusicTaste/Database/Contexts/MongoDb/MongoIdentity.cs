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

public class MongoIdentity : IIdentityProvider
{
    private readonly UserManager<MongoUser> _userManager;

    public MongoIdentity(UserManager<MongoUser> userManager)
    {
        _userManager = userManager;
    }

    public static void Configure(IServiceCollection services, string connectionString)
    {
        services.AddIdentity<MongoUser, MongoRole>()
            .AddMongoDbStores<MongoUser, MongoRole, ObjectId>(connectionString, "Security")
            .AddDefaultTokenProviders();
        
        services.AddScoped<IIdentityProvider, MongoIdentity>();
    }

    public async Task RegisterUserAsync(IRegisterUserDto newUser)
    {
        var mongoUser = new MongoUser
        {
            UserName = newUser.Username,
            Email = newUser.Email
        };
        
        var result = await _userManager.CreateAsync(mongoUser, newUser.Password);
        
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