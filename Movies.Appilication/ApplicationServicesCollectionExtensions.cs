using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Movies.Appilication.Repositories;
using Movies.Appilication.Validators;
using Movies.Application.Database;
using Movies.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.Appilication;

public static class ApplicationServicesCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMoviesRepository, MoviesRepository>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Scoped);
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
