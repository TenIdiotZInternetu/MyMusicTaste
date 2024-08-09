using MongoDB.Driver;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

public class SongSubmission : ISongSubmission
{
    public async Task SubmitSongAsync(Song songModel)
    {
        var song = songModel.ToFullModel();

        if (await AlreadyExistsAsync(song))
        {
            throw new EntryAlreadyExistsException("The submitted song already exists in the database.");
        }
        
        var collection = Song.Collection;
        await collection.InsertOneAsync(song);
    }
    
    private async Task<bool> AlreadyExistsAsync(Song song)
    {
        var builder = Builders<Song>.Filter;

        var filter = (builder.Eq(x => x.Title, song.Title) &
                      builder.Eq(x => x.Author, song.Author)) |
                     builder.Eq(x => x.Id, song.Id);
        
        var doc = await Song.Collection.Find(filter).FirstOrDefaultAsync();
        return doc != null;
    }

}