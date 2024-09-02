using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
using MongoDB.Driver;
using MyMusicTaste.Database.Operations;

namespace MyMusicTaste.Components.Pages;

public partial class SongPage : ComponentBase
{
    public const string RouteTemplate = "/songs/{SongId}";
    
    [Parameter]
    public string? SongId { get; set; }
    
    private enum PageState { Loading, Loaded, SongNotFound}
    private Models.Song? _song { get; set; }
    private PageState _pageState { get; set; } = PageState.Loading;

    public static string GetRoute(ObjectId songId)
    {
        return RouteTemplate.Replace("{SongId}", songId.ToString());
    }
    
    protected override void OnInitialized()
    {
        try
        {
            _song = SongRepository.GetById(SongId);
            _pageState = PageState.Loaded;
        }
        catch (EntryNotFoundException e)
        {
            _pageState = PageState.SongNotFound;
        }
    }
}