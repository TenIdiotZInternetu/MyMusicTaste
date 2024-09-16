using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb;

[CollectionName("Accounts")]
public class MongoAccount : MongoIdentityUser<ObjectId>
{
    public ObjectId UserId;
}

[CollectionName("Roles")]
public class MongoRole : MongoIdentityRole<ObjectId> {}

public class MongoIdentity : IIdentityProvider
{
    private readonly IDbRepository<User> _userRepository;
    private readonly SignInManager<MongoAccount> _signInManager;
    private UserManager<MongoAccount> _userManager => _signInManager.UserManager;

    public MongoIdentity(SignInManager<MongoAccount> signInManager, IDbRepository<User> userRepository)
    {
        _signInManager = signInManager;
        _userRepository = userRepository;
    }

    public static void Configure(IServiceCollection services, string connectionString)
    {
        services.AddIdentity<MongoAccount, MongoRole>()
            .AddMongoDbStores<MongoAccount, MongoRole, ObjectId>(connectionString, "Security")
            .AddDefaultTokenProviders();
        
        services.AddScoped<IIdentityProvider, MongoIdentity>();
    }

    public async Task<IdentityResult> SignUpUserAsync(IUserSignupDto newUserSignup)
    {
        var userId = new ObjectId();
        var result = await CreateAccountAsync(newUserSignup, userId);
        
        if (!result.Succeeded)
        {
            return result;
        }

        await CreateUserAsync(newUserSignup, userId);
        return result;
    }

    public Task<SignInResult> LoginUserAsync(IUserLoginDto userLogin)
    {
        return _signInManager
            .PasswordSignInAsync(userLogin.Username, userLogin.Password, true, false);
    }

    public Task<IdentityResult> AssignRoleAsync(IdentityUser user, IdentityRole role)
    {
        throw new NotImplementedException();
    }

    private async Task<IdentityResult> CreateAccountAsync(IUserSignupDto newUserSignup, ObjectId userId)
    {
        var mongoUser = new MongoAccount
        {
            UserName = newUserSignup.Username,
            Email = newUserSignup.Email,
            UserId = userId
        };
        
        return await _userManager.CreateAsync(mongoUser, newUserSignup.Password);
    }

    private async Task CreateUserAsync(IUserSignupDto newUserSignup, ObjectId userId)
    {
        var userModel = new User(newUserSignup.Username)
        {
            Id = userId
        };

        await _userRepository.CreateAsync(userModel);
    }
}