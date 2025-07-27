namespace MyMusicTaste.Components.Dialogs;

public struct DialogResult<TResult>
{
    public bool WasConfirmed { get; set; }
    public TResult? Result { get; set; }
}