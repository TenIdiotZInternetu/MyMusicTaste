using MongoDB.Driver;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

public class SongSubmission : ISongSubmission
{
    private static readonly MongoRepository<Song> _repository = new("Core", "Songs");
    
    public void SubmitSong(Song songModel)
    {
        throw new NotImplementedException();
    }

    public async Task SubmitSongAsync(Song song)
    {
        if (await AlreadyExistsAsync(song))
        {
            throw new EntryAlreadyExistsException("The submitted song already exists in the database.");
        }
        
        await _repository.CreateAsync(song);
    }
    
    private async Task<bool> AlreadyExistsAsync(Song song)
    {
        var builder = Builders<Song>.Filter;

        var filter = (builder.Eq(x => x.Title, song.Title) &
                      builder.Eq(x => x.Author, song.Author));
        
        var collection = _repository.Collection;
        var doc = collection.Find(filter).FirstOrDefault();
        return doc != null;
    }

}