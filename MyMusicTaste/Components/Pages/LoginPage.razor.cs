using Microsoft.AspNetCore.Components;

namespace MyMusicTaste.Components.Pages;

public partial class LoginPage : ComponentBase
{
    public const string RouteTemplate = "/login";
    public static string GetRoute() => RouteTemplate;
}