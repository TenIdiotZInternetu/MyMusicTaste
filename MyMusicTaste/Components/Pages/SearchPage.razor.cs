using Microsoft.AspNetCore.Components;
using Microsoft.Net.Http.Headers;
using MyMusicTaste.Components.SearchComps;

namespace MyMusicTaste.Components.Pages;

public partial class SearchPage : ComponentBase
{
    public const string RouteTemplate = "/search";

    private enum Tab {All, Songs, Users}

    private Tab _currentTab = Tab.All;

    private SongSearch _allTabSongSearch { get; set; }
    private UserSearch _allTabUserSearch { get; set; }
    private SongSearch _songTabSearch { get; set; }
    private UserSearch _userTabSearch { get; set; }
    
    public static string GetRoute() => RouteTemplate;
    
    private string _searchQuery { get; set; }
    
    private void RefreshSearches()
    {
        switch (_currentTab)
        {
            case Tab.All:
                _allTabSongSearch.UpdateResults(_searchQuery);
                break;
            case Tab.Songs:
                _songTabSearch.UpdateResults(_searchQuery);
                break;
            case Tab.Users:
                break;
        }
        
        StateHasChanged();
    }

    private void UpdateQuery(ChangeEventArgs e)
    {
        _searchQuery = e.Value?.ToString() ?? string.Empty;
        RefreshSearches();
    }

    private void ChangeTabToAll() => ChangeTab(Tab.All);
    private void ChangeTabToSongs() => ChangeTab(Tab.Songs);
    private void ChangeTabToUsers() => ChangeTab(Tab.Users);

    private void ChangeTab(Tab tab)
    {
        _currentTab = tab;
        RefreshSearches();
    }
}