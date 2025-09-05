using MyMusicTaste.Components;

namespace MyMusicTaste.Startup;

/// <summary>
/// Provides middleware configuration for the application.
/// </summary>
public static class MiddlewareSetup
{
    /// <summary>
    /// Configures middleware and request pipeline for the app.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    public static void Setup(WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();
        app.MapDefaultControllerRoute();
    }
}