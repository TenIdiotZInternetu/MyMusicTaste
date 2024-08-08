using MongoDB.Driver;
using MyMusicTaste.Database.Models;

namespace MyMusicTaste.Database.Operations;

public static class SongSubmission
{
    public static async Task SubmitSongAsync(ISongModel songModel)
    {
        var song = songModel.ToFullModel();

        if (await AlreadyExistsAsync(song))
        {
            throw new EntryAlreadyExistsException("The submitted song already exists in the database.");
        }
        
        var collection = SongFullModel.Collection;
        await collection.InsertOneAsync(song);
    }

    private static async Task<bool> AlreadyExistsAsync(SongFullModel song)
    {
        var builder = Builders<SongFullModel>.Filter;

        var filter = (builder.Eq(x => x.Title, song.Title) &
                      builder.Eq(x => x.Artist, song.Artist)) |
                      builder.Eq(x => x.Id, song.Id);
        
        var doc = await SongFullModel.Collection.Find(filter).FirstOrDefaultAsync();
        return doc != null;
    }
}