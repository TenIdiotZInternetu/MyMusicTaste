using MongoDB.Driver;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

public class SongSubmission : ISongSubmission
{
    private static readonly MongoRepository<Song> REPOSITORY = new();
    
    public void SubmitSong(Song songModel)
    {
        throw new NotImplementedException();
    }

    public async Task SubmitSongAsync(Song song)
    {
        if (AlreadyExists(song))
        {
            throw new EntryAlreadyExistsException("The submitted song already exists in the database.");
        }
        
        await REPOSITORY.CreateAsync(song);
    }
    
    private bool AlreadyExists(Song song)
    {
        var builder = Builders<Song>.Filter;

        var filter = builder.Eq(x => x.Title, song.Title) &
                     builder.Eq(x => x.Author, song.Author);
        
        var collection = REPOSITORY.Collection;
        var doc = collection.Find(filter).FirstOrDefault();
        return doc != null;
    }

}