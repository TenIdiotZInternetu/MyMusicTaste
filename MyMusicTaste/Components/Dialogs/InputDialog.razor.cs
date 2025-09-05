using Microsoft.AspNetCore.Components;

namespace MyMusicTaste.Components.Dialogs;

/// <summary>
/// Dialog component that allows user to input a string.
/// </summary>
public partial class InputDialog : Dialog<string>
{
    /// <summary>
    /// Placeholder text displayed inside the input area.
    /// </summary>
    [Parameter] public string Placeholder { get; set; } = "";
    
    private string _inputText = "";
    
    /// <summary>
    /// Returns the text entered in the dialog.
    /// </summary>
    protected override string GetResult() => _inputText;
}