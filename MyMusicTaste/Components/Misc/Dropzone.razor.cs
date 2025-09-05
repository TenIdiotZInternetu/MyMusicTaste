using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MyMusicTaste.Components.Misc;

/// <summary>
/// A component that represents a drag-and-drop zone for items. Dropzone must be activated before it can receive items.
/// </summary>
public partial class Dropzone : ComponentBase
{
    /// <summary>
    /// If true, the dropzone automatically activates when an item is dragged over it.
    /// </summary>
    [Parameter] public bool AutoActivate { get; set; } = true;
    
    /// <summary>
    /// If true, the dropzone automatically deactivates when an item leaves it.
    /// </summary>
    [Parameter] public bool AutoDeactivate { get; set; } = true;
    
    // TODO: Somehow propagate back DragEventArgs (if needed)
    /// <summary>
    /// Event triggered when an item is dragged into the dropzone.
    /// </summary>
    [Parameter] public EventCallback<Dropzone> OnDragEnter { get; set; }
    
    /// <summary>
    /// Event triggered when an item leaves the dropzone.
    /// </summary>
    [Parameter] public EventCallback<Dropzone> OnDragLeave { get; set; }
    
    /// <summary>
    /// Event triggered when an item is dropped onto the dropzone.
    /// </summary>
    [Parameter] public EventCallback<Dropzone> OnDrop { get; set; }

    private bool _active;
    private string _activeStyle => _active ? "active" : "inactive";

    /// <summary>
    /// Sets the active state of the dropzone.
    /// </summary>
    /// <param name="active">True to activate, false to deactivate.</param>
    public void SetActive(bool active)
    {
        _active = active;
        StateHasChanged();
    }

    private void Enter(DragEventArgs e)
    {
        OnDragEnter.InvokeAsync(this);
        if (AutoActivate)
        {
            SetActive(true);
        }
    }

    private void Leave(DragEventArgs e)
    {
        OnDragLeave.InvokeAsync(this);
        if (AutoDeactivate)
        {
            SetActive(false);
        }
    }

    private void Drop(DragEventArgs e)
    {
        if (!_active) return;
        OnDrop.InvokeAsync(this);
        SetActive(false);
    }
}