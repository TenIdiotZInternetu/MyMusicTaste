using Microsoft.AspNetCore.Components;

namespace MyMusicTaste.Components.Dialogs;

public partial class InputDialog : Dialog<string>
{
    [Parameter] public string Placeholder { get; set; } = "";
    private string _inputText = "";
    protected override string GetResult() => _inputText;
}