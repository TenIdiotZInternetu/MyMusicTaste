using Microsoft.AspNetCore.Components;
using MyMusicTaste.Utils;

namespace MyMusicTaste.Components.Misc;

/// <summary>
/// A component that displays an image thumbnail or a placeholder if the image link is invalid.
/// </summary>
public partial class Thumbnail : ComponentBase
{
    /// <summary>
    /// The URL of the image to display in the thumbnail.
    /// </summary>
    [Parameter] public string? ImageLink { get; set; }
    
    /// <summary>
    /// Alternative text to display if the image cannot be shown.
    /// </summary>
    [Parameter] public string? AltText { get; set; }
    
    // TODO: Create SuperComponent that does this automatically
    /// <summary>
    /// Additional HTML attributes to apply to the image or placeholder container.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UnmatchedAttributes { get; set; } = null!;

    private bool _isLinkValid;

    protected override async Task OnParametersSetAsync()
    {
        _isLinkValid = await LinkValidation.IsImageLinkValidAsync(ImageLink);
    }
}