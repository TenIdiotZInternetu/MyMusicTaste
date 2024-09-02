using Microsoft.AspNetCore.Components;
using Microsoft.Net.Http.Headers;
using MyMusicTaste.Components.SearchComps;

namespace MyMusicTaste.Components.Pages;

public partial class SearchPage : ComponentBase
{
    public const string RouteTemplate = "/search";

    private enum Tab {All, Songs, Users}

    private Tab _currentTab = Tab.All;

    private SongSearch AllTabSongSearch;
    private UserSearch AllTabUserSearch;
    private SongSearch SongTabSearch;
    private UserSearch UserTabSearch;
    
    
    public static string GetRoute() => RouteTemplate;

    private void UpdateSearches()
    {
        switch (_currentTab)
        {
            case Tab.All:
                
        }
    }

    private void ChangeTabToAll() => _currentTab = Tab.All;
    private void ChangeTabToSongs() => _currentTab = Tab.Songs;
    private void ChangeTabToUsers() => _currentTab = Tab.Users;
}