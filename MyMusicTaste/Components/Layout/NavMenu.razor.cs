using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MyMusicTaste.Components.Layout;

public partial class NavMenu : ComponentBase
{
    [Inject] private AuthenticationStateProvider _authState { get; set; } = null!;
    [Inject] private NavigationManager _navigation { get; set; } = null!;
    
    protected override void OnInitialized()
    {
        _authState.AuthenticationStateChanged += OnAuthStateChanged;
    }
    
    private void OnAuthStateChanged(Task<AuthenticationState> _)
    {
        StateHasChanged();
        _navigation.Refresh(forceReload: true);
    }
}