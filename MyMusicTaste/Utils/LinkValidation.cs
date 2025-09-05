using Microsoft.IdentityModel.Tokens;

namespace MyMusicTaste.Utils;

/// <summary>
/// Provides methods for validating external resource links such as images and songs.
/// </summary>
public static class LinkValidation
{
    /// <summary>
    /// Checks if a link points to a valid image resource using an HTTP HEAD request.
    /// </summary>
    /// <param name="link">The link to validate.</param>
    /// <param name="isEmptyValid">Whether an empty link is considered valid.</param>
    /// <returns>True if the link is a valid image; otherwise false.</returns>
    public static async Task<bool> IsImageLinkValidAsync(string? link, bool isEmptyValid = false)
    {
        if (link.IsNullOrEmpty())
        {
            return isEmptyValid;
        }

        try
        {
            var httpClient = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Head, link);
            using var response = await httpClient.SendAsync(request);

            string? mediaType = response.Content.Headers.ContentType?.MediaType;
            return mediaType?.StartsWith("image/", StringComparison.OrdinalIgnoreCase) ?? false;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Checks if a link points to a supported song source such as YouTube, Spotify, SoundCloud, or Apple Music.
    /// </summary>
    /// <param name="link">The link to validate.</param>
    /// <param name="isEmptyValid">Whether an empty link is considered valid.</param>
    /// <returns>True if the link matches a known song domain; otherwise false.</returns>
    public static bool IsSongLinkValid(string? link, bool isEmptyValid = false)
    {
        if (link.IsNullOrEmpty())
        {
            return isEmptyValid;
        }
        
        string[] validDomains =
        {
            "youtube.com/watch", 
            "music.youtube.com",
            "youtu.be/",
            "open.spotify.com/track",
            "soundcloud.com/",
            "music.apple.com/"
        };

        return validDomains.Any(link!.Contains);
    }
}