using Microsoft.AspNetCore.Components;

namespace MyMusicTaste.Components.Dialogs;

public abstract class Dialog<TResult> : ComponentBase
{
    [Parameter] public string? Title { get; set; }
    [Parameter] public string ConfirmText { get; set; } = "Confirm";
    [Parameter] public string CancelText { get; set; } = "Cancel";

    protected DialogBase BaseDialog { get; set; } = null!;
    protected TaskCompletionSource<bool> CompletionToken { get; private set; } = new();
    protected Task<bool> CompletionTask => CompletionToken.Task;

    protected override void OnAfterRender(bool firstRender)
    {
        BaseDialog.Title = Title;
        BaseDialog.ConfirmText = ConfirmText;
        BaseDialog.CancelText = CancelText;
        
        BaseDialog.ConfirmPressed += Confirm;
        BaseDialog.CancelPressed += Cancel;
    }
    
    public async Task<DialogResult<TResult>> OpenDialog()
    {
        CompletionToken = new();
        BaseDialog.Open();
        
        bool wasConfirmed = await CompletionTask;
        var result = new DialogResult<TResult>
        {
            WasConfirmed = wasConfirmed,
            Result = wasConfirmed ? GetResult() : default
        };

        BaseDialog.Close();
        return result;
    }
    
    public void Confirm()
    {
        CompletionToken.TrySetResult(true);
    }

    public void Cancel()
    {
        CompletionToken.TrySetResult(false);
    }
    
    protected abstract TResult GetResult();
}