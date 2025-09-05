using Microsoft.AspNetCore.Components;

namespace MyMusicTaste.Components.Dialogs;

/// <summary>
/// Dialog component for simple yes/no confirmation.
/// </summary>
public partial class ConfirmDialog : Dialog<bool>
{
    /// <summary>
    /// Question text to display in the dialog.
    /// </summary>
    [Parameter, EditorRequired] public string Question { get; set; } = null!;
    
    /// <summary>
    /// Returns the true if the dialog has been confirmed.
    /// </summary>
    protected override bool GetResult() => true;
}