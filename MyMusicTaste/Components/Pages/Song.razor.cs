using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
using MongoDB.Driver;
using MyMusicTaste.Database.Models;
using MyMusicTaste.Database.Operations;

namespace MyMusicTaste.Components.Pages;

public partial class Song : ComponentBase
{
    private enum PageState { Loading, Loaded, SongNotFound}
    
    [Parameter]
    public string? SongId { get; set; }
    
    private SongFullModel? _song { get; set; }
    private PageState _pageState { get; set; } = PageState.Loading;
    
    protected override void OnInitialized()
    {
        try
        {
            LoadSongModel();
            _pageState = PageState.Loaded;
        }
        catch (EntryNotFoundException e)
        {
            _pageState = PageState.SongNotFound;
        }
    }

    private void LoadSongModel()
    {
        bool idIsValid = ObjectId.TryParse(SongId, out ObjectId objectId);
        
        if (!idIsValid)
        {
            throw new EntryNotFoundException("Invalid song ID!");
        }
        
        var collection = SongFullModel.Collection;
        var filter = Builders<SongFullModel>.Filter
            .Eq(x => x.Id, objectId);

        _song = collection.Find(filter).FirstOrDefault();

        if (_song == null)
        {
            throw new EntryNotFoundException("Song not found!");
        }
    }
}