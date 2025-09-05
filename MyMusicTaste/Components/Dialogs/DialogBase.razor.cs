using Microsoft.AspNetCore.Components;

namespace MyMusicTaste.Components.Dialogs;

/// <summary>
/// Provides a HTML wrapper for dialogs with header, body, and footer containing confirm/cancel buttons.
/// </summary>
public partial class DialogBase : ComponentBase
{
    /// <summary>
    /// Content to display inside the dialog body.
    /// </summary>
    [Parameter, EditorRequired] public RenderFragment BodyContent { get; set; } = null!;
    
    /// <summary>
    /// Title of the dialog.
    /// </summary>
    public string? Title { get; set; }
    
    /// <summary>
    /// Text for the confirm button.
    /// </summary>
    public string ConfirmText { get; set; } = "Yes";
    
    /// <summary>
    /// Text for the cancel button.
    /// </summary>
    public string CancelText { get; set; } = "No";
    
    /// <summary>
    /// Indicates whether the dialog is currently visible.
    /// </summary>
    public bool IsVisible { get; private set; }

    /// <summary>
    /// Event triggered when the confirm button is pressed.
    /// </summary>
    public event Action? ConfirmPressed;
    
    /// <summary>
    /// Event triggered when the cancel button is pressed.
    /// </summary>
    public event Action? CancelPressed;

    /// <summary>
    /// Makes the dialog visible.
    /// </summary>
    public void Open()
    {
        IsVisible = true;
        StateHasChanged();
    }

    /// <summary>
    /// Makes the dialog invisible.
    /// </summary>
    public void Close()
    {
        IsVisible = false;
        StateHasChanged();
    }
    
    private void Confirm()
    {
        ConfirmPressed?.Invoke();
    }

    private void Cancel()
    {
        CancelPressed?.Invoke();
    }
}