using System.Diagnostics;
using Microsoft.AspNetCore.Components;

namespace MyMusicTaste.Components.Pages;

public partial class ErrorPage : ComponentBase
{
    public const string RouteTemplate = "/error";
    
    [CascadingParameter] private HttpContext? HttpContext { get; set; }

    private string? RequestId { get; set; }
    private bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public static string GetRoute() => RouteTemplate;
    
    protected override void OnInitialized()
    {
        RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
    }
}