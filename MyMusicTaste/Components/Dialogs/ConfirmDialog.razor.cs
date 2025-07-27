using Microsoft.AspNetCore.Components;

namespace MyMusicTaste.Components.Dialogs;

public partial class ConfirmDialog : ComponentBase
{
    [Parameter] public string? Title { get; set; }
    [Parameter, EditorRequired] public string Question { get; set; } = null!;
    [Parameter] public string ConfirmText { get; set; } = "Yes";
    [Parameter] public string CancelText { get; set; } = "No";

    private DialogBase _dialogBase = null!;
    private TaskCompletionSource<bool> _completionToken => _dialogBase.CompletionToken;
    private Task<bool> _completionTask => _completionToken.Task;
    
    public async Task<bool> OpenDialog()
    {
        _dialogBase.Open();
        var result = await _completionTask;
        
        _dialogBase.Close();
        return result;
    }

    private void Confirm()
    {
        _completionToken.TrySetResult(true);
    }

    private void Cancel()
    {
        _completionToken.TrySetResult(false);
    }
}