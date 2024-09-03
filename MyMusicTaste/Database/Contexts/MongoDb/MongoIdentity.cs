using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb;

[CollectionName("Accounts")]
public class MongoUser : MongoIdentityUser<ObjectId> {}

[CollectionName("Roles")]
public class MongoRole : MongoIdentityRole<ObjectId> {}

public class MongoIdentity : IIdentityProvider
{
    private readonly UserManager<MongoUser> _userManager;
    private readonly IDbRepository<User> _userRepository;

    public MongoIdentity(UserManager<MongoUser> userManager, IDbRepository<User> userRepository)
    {
        _userManager = userManager;
        _userRepository = userRepository;
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
            throw new UserRegistrationFailedException(result.Errors);
        }

        var userModel = new User(newUser.Username);
        await _userRepository.CreateAsync(userModel);
    }

    public async Task AssignRoleAsync(MongoUser user, MongoRole role)
    {
        throw new NotImplementedException();
    }
}