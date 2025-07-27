namespace MyMusicTaste.Components.Dialogs;

public partial class InputDialog : Dialog<string>
{
    private string _inputText = "";
    protected override string GetResult() => _inputText;
}