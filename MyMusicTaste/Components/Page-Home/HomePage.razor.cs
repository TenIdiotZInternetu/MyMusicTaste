using Microsoft.AspNetCore.Components;

namespace MyMusicTaste.Components.Page_Home;

public partial class HomePage : ComponentBase
{
    public const string ROUTE_TEMPLATE = "/";
    public static string GetRoute() => ROUTE_TEMPLATE;
}