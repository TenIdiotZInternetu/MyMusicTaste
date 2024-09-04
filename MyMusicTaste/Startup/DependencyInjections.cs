using MyMusicTaste.Database;
using MyMusicTaste.Database.Contexts.MongoDb;
using MyMusicTaste.Database.Contexts.MongoDb.Operations;
using MyMusicTaste.Database.Operations;
using MyMusicTaste.Models;

namespace MyMusicTaste;

public static class DependencyInjections
{
    public static void InjectDependencies(this IServiceCollection services)
    {
        services.InjectDbRepositories();
        services.InjectDbOperations();
    }
    
    private static void InjectDbRepositories(this IServiceCollection services)
    {
        services.AddTransient<IDbRepository<Song>, MongoRepository<Song>>();
        services.AddTransient<IDbRepository<User>, MongoRepository<User>>();
    }
    
    private static void InjectDbOperations(this IServiceCollection services)
    {
        services.AddSingleton<ISongSubmission, SongSubmission>();
        services.AddTransient<ISearchOperation<Song>, MongoSongSearch>();
        services.AddTransient<ISearchOperation<User>, MongoUserSearch>();
        services.AddSingleton<ISongRatingListing, MongoSongRatingListing>();
    }
}