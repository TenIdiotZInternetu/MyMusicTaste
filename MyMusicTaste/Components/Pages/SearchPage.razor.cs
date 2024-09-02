using Microsoft.AspNetCore.Components;

namespace MyMusicTaste.Components.Pages;

public partial class SearchPage : ComponentBase
{
    public const string RouteTemplate = "/search";
    
    public static string GetRoute() => RouteTemplate;
}