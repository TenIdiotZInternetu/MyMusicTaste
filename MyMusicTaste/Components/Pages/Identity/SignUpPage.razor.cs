using Microsoft.AspNetCore.Components;

namespace MyMusicTaste.Components.Pages.Identity;

public partial class SignupPage : ComponentBase
{
    public const string RouteTemplate = "/signup";
    public static string GetRoute() => RouteTemplate;
}