using MongoDB.Bson;
using MongoDB.Driver;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

public class SongSubmission : ISongSubmission
{
    private static readonly MongoRepository<Song> REPOSITORY = new();

    public async Task<string> SubmitSongAsync(Song song)
    {
        if (AlreadyExists(song))
        {
            throw new EntryAlreadyExistsException("The submitted song already exists in the database.");
        }
        
        song.Id = ObjectId.GenerateNewId();
        await REPOSITORY.CreateAsync(song);
        return song.Id.ToString();
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