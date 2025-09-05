using Microsoft.AspNetCore.Components;

namespace MyMusicTaste.Components.Page_Auth;

/// <summary>
/// Page for users to login or signup.
/// </summary>
public partial class LoginPage : ComponentBase
{
    public const string ROUTE_TEMPLATE = "/login";
    public static string GetRoute() => ROUTE_TEMPLATE;
}