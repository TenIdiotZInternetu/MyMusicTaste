using Microsoft.AspNetCore.Identity;

namespace MyMusicTaste.Database;

/// <summary>
/// Provides an abstraction for user authentication, signup, login, logout, and role management.
/// </summary>
public interface IIdentityProvider
{
    /// <summary>
    /// Signs up a new user.
    /// </summary>
    /// <param name="userSignup">The user signup data.</param>
    /// <returns>A task for the <see cref="IdentityResult"/> indicating success or failure.</returns>
    public Task<IdentityResult> SignUpUserAsync(IUserSignupDto userSignup);
    
    /// <summary>
    /// Logs in a user.
    /// </summary>
    /// <param name="userLoginDto">The user login data.</param>
    /// <returns>A task for the <see cref="SignInResult"/> indicating success or failure.</returns>
    public Task<SignInResult> LoginUserAsync(IUserLoginDto userLoginDto);
    
    /// <summary>
    /// Logs out the currently signed-in user.
    /// </summary>
    public Task LogOutUserAsync();
    
    /// <summary>
    /// Assigns a role to a user.
    /// </summary>
    /// <param name="user">The user to assign the role to.</param>
    /// <param name="role">The role to assign.</param>
    /// <returns>A task for the <see cref="IdentityResult"/>.</returns>
    public Task<IdentityResult> AssignRoleAsync(IdentityUser user, IdentityRole role);
    
    /// <summary>
    /// Checks if a user is authenticated.
    /// </summary>
    /// <returns>True if a user is authenticated; otherwise false.</returns>
    public bool IsAuthenticated();
    
    /// <summary>
    /// Checks if a signed-in user has a specific ID.
    /// </summary>
    /// <param name="requiredUserId">The required user ID.</param>
    /// <returns>True if the user has the specified ID; otherwise false.</returns>
    public bool AuthorizeUserById(string requiredUserId);
    
    /// <summary>
    /// Gets the currently signed-in user's ID.
    /// </summary>
    /// <returns>The user ID as a string, or null if not authenticated.</returns>
    public string? GetUserId();
}

/// <summary>
/// Data required to sign up a user.
/// </summary>
public interface IUserSignupDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

/// <summary>
/// Data required to log in a user.
/// </summary>
public interface IUserLoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}