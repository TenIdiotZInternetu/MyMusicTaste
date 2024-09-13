using Microsoft.AspNetCore.Components;

namespace MyMusicTaste.Components.Misc;

public partial class Thumbnail : ComponentBase
{
    [Parameter]
    public string? ImageLink { get; set; }

    private bool _validLink => !string.IsNullOrEmpty(ImageLink);
}