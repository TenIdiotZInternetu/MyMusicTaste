namespace MyMusicTaste.Components.Dialogs;

/// <summary>
/// Represents the result of a dialog.
/// </summary>
/// <typeparam name="TResult">Type of the value returned when confirmed.</typeparam>
public struct DialogResult<TResult>
{
    /// <summary>
    /// Indicates if the dialog was confirmed.
    /// </summary>
    public bool WasConfirmed { get; set; }
    
    /// <summary>
    /// The value returned if the dialog was confirmed; otherwise default.
    /// </summary>
    public TResult? Result { get; set; }
}