using Microsoft.AspNetCore.Components;

namespace MyMusicTaste.Components.Page_Search;

public partial class SearchPage : ComponentBase
{
    public const string RouteTemplate = "/search";

    private enum Tab {All, Songs, Users}

    private Tab _currentTab = Tab.All;
    
    public static string GetRoute() => RouteTemplate;

    private string _searchQuery { get; set; } = string.Empty;

    private void UpdateQuery(ChangeEventArgs e)
    {
        _searchQuery = e.Value?.ToString() ?? string.Empty;
        StateHasChanged();
    }

    private void ChangeTabToAll() => ChangeTab(Tab.All);
    private void ChangeTabToSongs() => ChangeTab(Tab.Songs);
    private void ChangeTabToUsers() => ChangeTab(Tab.Users);

    private void ChangeTab(Tab tab)
    {
        _currentTab = tab;
        StateHasChanged();
    }

    private string TabIsActive(Tab tab)
    {
        return tab == _currentTab ? "active" : "";
    }
}