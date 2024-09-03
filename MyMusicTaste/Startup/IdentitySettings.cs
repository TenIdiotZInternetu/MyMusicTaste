using Microsoft.AspNetCore.Identity;

namespace MyMusicTaste;

public static class IdentitySettings
{
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
    }
}