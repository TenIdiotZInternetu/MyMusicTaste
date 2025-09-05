using MongoDB.Bson;
using MongoDB.Driver;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

/// <summary>
/// Handles submission of new songs to the MongoDB database, ensuring no duplicates exist.
/// </summary>
public class SongSubmission : ISongSubmission
{
    private static readonly MongoRepository<Song> REPOSITORY = new();

    /// <summary>
    /// Submits a new song to the database.
    /// </summary>
    /// <param name="song">The song to submit.</param>
    /// <returns>A task for the ID of the newly created song as a string.</returns>
    /// <exception cref="EntryAlreadyExistsException">Thrown if the song already exists in the repository.</exception>
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