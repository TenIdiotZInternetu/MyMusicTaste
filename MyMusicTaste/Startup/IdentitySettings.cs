using Microsoft.AspNetCore.Identity;

namespace MyMusicTaste.Startup;

/// <summary>
/// Provides configuration for ASP.NET Identity settings.
/// </summary>
public static class IdentitySettings
{
    /// <summary>
    /// Sets up identity options such as password rules, unique email requirement,
    /// and enables cascading authentication state.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static void Setup(IServiceCollection services)
    {
        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;

            options.Password.RequireNonAlphanumeric = false;
        });
        
        services.AddCascadingAuthenticationState();
    }
}