using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MyMusicTaste.Components.Misc;

public partial class Dropzone : ComponentBase
{
    [Parameter] public bool AutoActivate { get; set; } = true;
    [Parameter] public bool AutoDeactivate { get; set; } = true;
    
    // TODO: Somehow propagate back DragEventArgs (if needed)
    [Parameter] public EventCallback<Dropzone> OnDragEnter { get; set; }
    [Parameter] public EventCallback<Dropzone> OnDragLeave { get; set; }
    [Parameter] public EventCallback<Dropzone> OnDrop { get; set; }

    private bool _active;
    private string style => _active ? "active" : "inactive";

    public void SetActive(bool active)
    {
        _active = active;
        StateHasChanged();
    }

    private void Enter(DragEventArgs e)
    {
        OnDragEnter.InvokeAsync();
        if (AutoActivate)
        {
            SetActive(true);
        }
    }

    private void Leave(DragEventArgs e)
    {
        OnDragLeave.InvokeAsync();
        if (AutoDeactivate)
        {
            SetActive(false);
        }
    }

    private void Drop(DragEventArgs e)
    {
        if (!_active) return;
        OnDrop.InvokeAsync();
        SetActive(false);
    }
}