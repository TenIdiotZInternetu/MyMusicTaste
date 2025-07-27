using Microsoft.AspNetCore.Components;

namespace MyMusicTaste.Components.Dialogs;

public partial class DialogBase : ComponentBase
{
    [Parameter] public string? Title { get; set; }
    [Parameter] public RenderFragment? BodyContent { get; set; }
    [Parameter] public RenderFragment? FooterContent { get; set; }
    [Parameter] public bool ClickableBackdrop { get; set; } = true;
    public TaskCompletionSource<bool> CompletionToken { get; private set; } = new();

    private bool _isVisible;

    public void Open()
    {
        CompletionToken = new();
        _isVisible = true;
        StateHasChanged();
    }

    public void Close()
    {
        _isVisible = false;
        StateHasChanged();
    }
    
    private void OnClickBackdrop()
    {
        if (ClickableBackdrop)
        {
            CompletionToken.TrySetResult(false);
        }
    }
}