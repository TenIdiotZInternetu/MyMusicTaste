using Microsoft.IdentityModel.Tokens;

namespace MyMusicTaste.Utils;

public static class LinkValidation
{
    public static async Task<bool> IsImageLinkValidAsync(string? link)
    {
        if (link.IsNullOrEmpty())
        {
            return false;
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

    public static bool IsSongLinkValid(string? link)
    {
        if (link.IsNullOrEmpty())
        {
            return false;
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