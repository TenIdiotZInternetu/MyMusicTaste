using System.Diagnostics;
using Microsoft.AspNetCore.Components;

namespace MyMusicTaste.Components.Pages;

public partial class HomePage : ComponentBase
{
    public const string RouteTemplate = "/";
    
    public static string GetRoute() => RouteTemplate;
}