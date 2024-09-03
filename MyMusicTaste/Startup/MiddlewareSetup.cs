using MyMusicTaste.Components;

namespace MyMusicTaste;

public static class MiddlewareSetup
{
    public static void Setup(WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();
    }
}