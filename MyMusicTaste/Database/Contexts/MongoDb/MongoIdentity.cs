using System.Security.Claims;
using Amazon.Auth.AccessControlPolicy;
using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb;

[CollectionName("Accounts")]
public class MongoAccount : MongoIdentityUser<ObjectId> {}

[CollectionName("Roles")]
public class MongoRole : MongoIdentityRole<ObjectId> {}

public class MongoIdentity : IIdentityProvider
{
    private const string USER_ID_CLAIM = "UserId";

    private readonly IDbRepository<User> _userRepository;
    private readonly SignInManager<MongoAccount> _signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    private UserManager<MongoAccount> _userManager => _signInManager.UserManager;

    public MongoIdentity(
        SignInManager<MongoAccount> signInManager, 
        IDbRepository<User> userRepository, 
        IHttpContextAccessor httpContextAccessor)
    {
        _signInManager = signInManager;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
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
        var userId = ObjectId.GenerateNewId();
        var mongoUser = new MongoAccount
        {
            UserName = newUserSignup.Username,
            Email = newUserSignup.Email,
        };
        
        var result = await _userManager.CreateAsync(mongoUser, newUserSignup.Password);
        if (!result.Succeeded) return result;

        Task.WaitAll(
            _userManager.AddClaimAsync(mongoUser, new Claim(USER_ID_CLAIM, userId.ToString())),
            CreateUserAsync(newUserSignup, userId)
        );

        return result;
    }

    public async Task<SignInResult> LoginUserAsync(IUserLoginDto userLogin)
    {
        return await _signInManager.PasswordSignInAsync(userLogin.Username, userLogin.Password, true, false);
    }

    public async Task LogOutUserAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public Task<IdentityResult> AssignRoleAsync(IdentityUser user, IdentityRole role)
    {
        throw new NotImplementedException();
    }

    public bool AuthorizeUserById(string requiredUserId)
    {
        return _httpContextAccessor.HttpContext?.User.HasClaim(USER_ID_CLAIM, requiredUserId) ?? false;
    }

    private async Task CreateUserAsync(IUserSignupDto newUserSignup, ObjectId userId)
    {
        var userModel = new User
        {
            Username = newUserSignup.Username,
            Id = userId
        };

        await _userRepository.CreateAsync(userModel);
    }
}