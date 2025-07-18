using Microsoft.AspNetCore.Components;

namespace MyMusicTaste.Components.Page_Home;

public partial class HomePage : ComponentBase
{
    public const string RouteTemplate = "/";
    
    public static string GetRoute() => RouteTemplate;
}