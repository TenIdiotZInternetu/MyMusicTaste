using System.Security.Claims;
using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb;

[CollectionName("Accounts")]
public class MongoAccount : MongoIdentityUser<ObjectId> {}

[CollectionName("Roles")]
public class MongoRole : MongoIdentityRole<ObjectId> {}

/// <summary>
/// A MongoDB-based implementation of <see cref="IIdentityProvider"/> using ASP.NET Identity.
/// Handles user signup, login, logout, and authentication/authorization checks.
/// </summary>
public class MongoIdentity : IIdentityProvider
{
    private const string USER_ID_CLAIM = "UserId";

    private readonly IDbRepository<User> _userRepository;
    private readonly SignInManager<MongoAccount> _signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    private UserManager<MongoAccount> _userManager => _signInManager.UserManager;

    /// <summary>
    /// Initializes a new instance of <see cref="MongoIdentity"/> with required ASP.NET Identity services.
    /// </summary>
    /// <param name="signInManager">The SignInManager for user login/logout.</param>
    /// <param name="userRepository">Repository for storing application user data.</param>
    /// <param name="httpContextAccessor">Accessor for the current HTTP context.</param>
    public MongoIdentity(
        SignInManager<MongoAccount> signInManager, 
        IDbRepository<User> userRepository, 
        IHttpContextAccessor httpContextAccessor)
    {
        _signInManager = signInManager;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Configures ASP.NET Identity services and registers <see cref="MongoIdentity"/> as the identity provider.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="connectionString">The MongoDB connection string for the identity store.</param>
    public static void Configure(IServiceCollection services, string connectionString)
    {
        services.AddIdentity<MongoAccount, MongoRole>()
            .AddMongoDbStores<MongoAccount, MongoRole, ObjectId>(connectionString, "Security")
            .AddDefaultTokenProviders();
        
        services.AddScoped<IIdentityProvider, MongoIdentity>();
    }

    /// <summary>
    /// Signs up a new user and stores their account in MongoDB.
    /// </summary>
    /// <param name="newUserSignup">The user signup data transfer object.</param>
    /// <returns>A task for the <see cref="IdentityResult"/> indicating success or failure.</returns>
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

    /// <summary>
    /// Logs in a user using ASP.NET Identity sign-in manager.
    /// </summary>
    /// <param name="userLogin">The user login data transfer object.</param>
    /// <returns>A task for the <see cref="SignInResult"/> indicating success or failure.</returns>
    public async Task<SignInResult> LoginUserAsync(IUserLoginDto userLogin)
    {
        return await _signInManager.PasswordSignInAsync(userLogin.Username, userLogin.Password, true, false);
    }

    /// <summary>
    /// Logs out the currently signed-in user.
    /// </summary>
    public async Task LogOutUserAsync()
    {
        await _signInManager.SignOutAsync();
    }

    /// <summary>
    /// Assigns a role to a user. (Not implemented in this MongoDB version.)
    /// </summary>
    /// <param name="user">The identity user to assign the role to.</param>
    /// <param name="role">The identity role to assign.</param>
    /// <returns>A task for the <see cref="IdentityResult"/>.</returns>
    public Task<IdentityResult> AssignRoleAsync(IdentityUser user, IdentityRole role)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Checks if the current HTTP context has an authenticated user.
    /// </summary>
    /// <returns>True if a user is authenticated; otherwise false.</returns>
    public bool IsAuthenticated()
    {
        return _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }

    /// <summary>
    /// Checks if the current user has a specific user ID claim.
    /// </summary>
    /// <param name="requiredUserId">The required user ID.</param>
    /// <returns>True if the user has the claim; otherwise false.</returns>
    public bool AuthorizeUserById(string requiredUserId)
    {
        return _httpContextAccessor.HttpContext?.User.HasClaim(USER_ID_CLAIM, requiredUserId) ?? false;
    }

    /// <summary>
    /// Retrieves the current user's ID from the HTTP context.
    /// </summary>
    /// <returns>The user ID as a string, or null if not authenticated.</returns>
    public string? GetUserId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirstValue(USER_ID_CLAIM);
    }

    /// <summary>
    /// Stores the user in the application repository.
    /// </summary>
    /// <param name="newUserSignup">The signup data.</param>
    /// <param name="userId">The generated user ID.</param>
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