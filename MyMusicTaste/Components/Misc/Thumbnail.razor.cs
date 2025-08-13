using Microsoft.AspNetCore.Components;
using MyMusicTaste.Utils;

namespace MyMusicTaste.Components.Misc;

public partial class Thumbnail : ComponentBase
{
    [Parameter] public string? ImageLink { get; set; }
    [Parameter] public string? AltText { get; set; }

    private bool _isLinkValid;

    protected override async Task OnParametersSetAsync()
    {
        _isLinkValid = await LinkValidation.IsImageLinkValidAsync(ImageLink);
    }
}