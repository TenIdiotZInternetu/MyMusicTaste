using MyMusicTaste.Database;
using MyMusicTaste.Database.Contexts.MongoDb;
using MyMusicTaste.Database.Contexts.MongoDb.Operations;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste.Startup;

/// <summary>
/// Provides extension methods for dependency injection setup.
/// </summary>
public static class DependencyInjections
{
    /// <summary>
    /// Registers application services and dependencies.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static void InjectDependencies(this IServiceCollection services)
    {
        services.InjectDbRepositories();
        services.InjectDbOperations();
    }
    
    private static void InjectDbRepositories(this IServiceCollection services)
    {
        services.AddTransient<IDbRepository<Song>, MongoRepository<Song>>();
        services.AddTransient<IDbRepository<User>, MongoRepository<User>>();
        services.AddTransient<IDbRepository<SongRating>, MongoRepository<SongRating>>();
        services.AddTransient<IDbRepository<Comment>, MongoRepository<Comment>>();
    }
    
    private static void InjectDbOperations(this IServiceCollection services)
    {
        services.AddSingleton<ISongSubmission, SongSubmission>();
        services.AddTransient<ISearchOperation<Song>, MongoSongSearch>();
        services.AddTransient<ISearchOperation<User>, MongoUserSearch>();
        services.AddSingleton<ISongRatingListing, MongoSongRatingListing>();
        services.AddSingleton<ICommentsListing, MongoCommentsListing>();
        services.AddTransient<ISongStatsCalculation, MongoSongsStatsCalculation>();
        services.AddTransient<IUserStatsCalculation, MongoUserStatsCalculation>();
    }
}