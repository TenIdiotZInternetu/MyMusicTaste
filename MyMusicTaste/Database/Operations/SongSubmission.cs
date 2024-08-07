using MongoDB.Driver;
using MyMusicTaste.Database.Models;

namespace MyMusicTaste.Database.Operations;

public static class SongSubmission
{
    public static Task SubmitSong(ISongModel songModel)
    {
        var song = songModel.ToFullModel();

        if (AlreadyExists(song))
        {
            var exception = new DatabaseOperationException("The submitted song already exists in the database.");
            return Task.FromException(exception);
        }
        
        var collection = SongFullModel.Collection;
        collection.InsertOne(song);
        return Task.CompletedTask;
    }

    private static bool AlreadyExists(SongFullModel song)
    {
        var builder = Builders<SongFullModel>.Filter;

        var filter = (builder.Eq(x => x.Title, song.Title) &
                      builder.Eq(x => x.Artist, song.Artist)) |
                      builder.Eq(x => x.Id, song.Id);
        
        var doc = SongFullModel.Collection.Find(filter).FirstOrDefault();
        return doc != null;
    }
}