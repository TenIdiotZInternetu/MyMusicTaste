using MongoDB.Bson;
using MongoDB.Driver;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Database.Contexts.MongoDb.Operations;

/// <summary>
/// Retrieves song ratings from a MongoDB collection.
/// </summary>
public class MongoSongRatingListing : ISongRatingListing
{
    private IMongoCollection<SongRating> _collection = MongoCollectionFactory.Create<SongRating>();

    /// <summary>
    /// Retrieves the rating a specific user has given to a specific song.
    /// </summary>
    /// <param name="songId">The ID of the song.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task for the retrieved song rating, or null if no rating exists.</returns>
    public Task<SongRating> GetSongRatingAsync(string songId, string userId)
    {
        var filter = Builders<SongRating>.Filter
            .Where(rating => rating.SongId.ToString() == songId &&
                             rating.UserId.ToString() == userId);
        
        return _collection.Find(filter).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Retrieves all ratings submitted by a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task for the collection of ratings.</returns>
    public async Task<IEnumerable<SongRating>> GetRatingsByUserAsync(string userId)
    {
        var filter = Builders<SongRating>.Filter
            .Eq(rating => rating.UserId, new ObjectId(userId));
        
        return await _collection.Find(filter).ToListAsync();
    }

    /// <summary>
    /// Retrieves all ratings associated with a specific song.
    /// </summary>
    /// <param name="songId">The ID of the song.</param>
    /// <returns>A task for the collection of ratings.</returns>
    public async Task<IEnumerable<SongRating>> GetRatingsBySongAsync(string songId)
    {
        var filter = Builders<SongRating>.Filter
            .Eq(rating => rating.SongId, new ObjectId(songId));
        
        return await _collection.Find(filter).ToListAsync();
    }
}