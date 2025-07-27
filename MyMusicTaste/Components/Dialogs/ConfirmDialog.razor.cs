using Microsoft.AspNetCore.Components;

namespace MyMusicTaste.Components.Dialogs;

public partial class ConfirmDialog : Dialog<bool>
{
    [Parameter, EditorRequired] public string Question { get; set; } = null!;
    protected override bool GetResult() => true;
}