using Microsoft.AspNetCore.Components;

namespace MyMusicTaste.Components.Pages.Identity;

public partial class SignInPage : ComponentBase
{
    public const string RouteTemplate = "/signin";
    public static string GetRoute() => RouteTemplate;
}