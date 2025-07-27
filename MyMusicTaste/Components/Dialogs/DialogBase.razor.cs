using Microsoft.AspNetCore.Components;
using Mono.TextTemplating;

namespace MyMusicTaste.Components.Dialogs;

public partial class DialogBase : ComponentBase
{
    [Parameter, EditorRequired] public RenderFragment BodyContent { get; set; } = null!;
    public string? Title { get; set; }
    public string ConfirmText { get; set; } = "Yes";
    public string CancelText { get; set; } = "No";
    
    public bool IsVisible { get; private set; }

    public event Action? ConfirmPressed;
    public event Action? CancelPressed;

    public void Open()
    {
        IsVisible = true;
        StateHasChanged();
    }

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