using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Movies.Appilication.Repositories;
using Movies.Appilication.Validators;
using Movies.Application.Database;
using Movies.Application.Repositories;
using Movies.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.Appilication;

public static class ApplicationServicesCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IRatingRepository, RatingRepository>();
        services.AddSingleton<IRatingService, RatingService>();
        services.AddSingleton<IMoviesRepository, MoviesRepository>();
        services.AddSingleton<IMovieService, MovieService>();
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
        return services;
    }
    public static IServiceCollection AddDatabase(this IServiceCollection services,
        string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ =>
            new NpgsqlConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        return services;
    }
}
