using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
using MyMusicTaste.Database.Operations;

namespace MyMusicTaste.Components.Page_Song;

public partial class SongPage : ComponentBase
{
    public const string ROUTE_TEMPLATE = "/songs/{SongId}";
    
    [Parameter]
    public string? SongId { get; set; }
    
    private enum PageState { Loading, Loaded, SongNotFound }

    private PageState _pageState = PageState.Loading;
    private Models.Song? _song;

    public static string GetRoute(ObjectId songId)
    {
        return ROUTE_TEMPLATE.Replace("{SongId}", songId.ToString());
    }
    
    protected override void OnInitialized()
    {
        try
        {
            _song = SongRepository.GetById(SongId);
            _pageState = PageState.Loaded;
        }
        catch (EntryNotFoundException)
        {
            _pageState = PageState.SongNotFound;
        }
    }
}