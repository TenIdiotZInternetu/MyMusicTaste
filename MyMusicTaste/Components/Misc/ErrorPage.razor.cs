using System.Diagnostics;
using Microsoft.AspNetCore.Components;

namespace MyMusicTaste.Components.Misc;

public partial class ErrorPage : ComponentBase
{
    public const string ROUTE_TEMPLATE = "/error";
    
    [CascadingParameter] private HttpContext? HttpContext { get; set; }

    private string? RequestId { get; set; }
    private bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public static string GetRoute() => ROUTE_TEMPLATE;
    
    protected override void OnInitialized()
    {
        RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
    }
}