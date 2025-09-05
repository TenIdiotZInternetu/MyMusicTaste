using Microsoft.AspNetCore.Components;

namespace MyMusicTaste.Components.Dialogs;

/// <summary>
/// Base abstract class for all dialogs. Manages opening, closing, and confirmation/cancellation.
/// </summary>
/// <typeparam name="TResult">The type of the result returned when the dialog is confirmed.</typeparam>
public abstract class Dialog<TResult> : ComponentBase
{
    /// <summary>
    /// Title of the dialog.
    /// </summary>
    [Parameter] public string? Title { get; set; }
    
    /// <summary>
    /// Text for the confirm button.
    /// </summary>
    [Parameter] public string ConfirmText { get; set; } = "Confirm";
    
    /// <summary>
    /// Text for the cancel button.
    /// </summary>
    [Parameter] public string CancelText { get; set; } = "Cancel";

    /// <summary>
    /// Reference to the underlying HTML wrapper dialog component.
    /// </summary>
    protected DialogBase BaseDialog { get; set; } = null!;
    
    /// <summary>
    /// Token used to track dialog completion.
    /// </summary>
    protected TaskCompletionSource<bool> CompletionToken { get; private set; } = new();
    
    /// <summary>
    /// Task representing the completion of the dialog.
    /// </summary>
    protected Task<bool> CompletionTask => CompletionToken.Task;
    
    protected override void OnAfterRender(bool firstRender)
    {
        BaseDialog.Title = Title;
        BaseDialog.ConfirmText = ConfirmText;
        BaseDialog.CancelText = CancelText;
        
        BaseDialog.ConfirmPressed += Confirm;
        BaseDialog.CancelPressed += Cancel;
    }
    
    /// <summary>
    /// Opens the dialog and returns a <see cref="DialogResult{TResult}"/> when confirmed or canceled.
    /// </summary>
    /// <returns>A task for the result of the dialog</returns>
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
    
    /// <summary>
    /// Marks the dialog as confirmed.
    /// </summary>
    public void Confirm()
    {
        CompletionToken.TrySetResult(true);
    }

    /// <summary>
    /// Marks the dialog as canceled.
    /// </summary>
    public void Cancel()
    {
        CompletionToken.TrySetResult(false);
    }
    
    /// <summary>
    /// Returns the result of the dialog when confirmed.
    /// </summary>
    protected abstract TResult GetResult();
}