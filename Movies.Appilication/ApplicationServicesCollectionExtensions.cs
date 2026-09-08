using Microsoft.Extensions.DependencyInjection;
using Movies.Appilication.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.Appilication;

public static class ApplicationServicesCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMoviesRepository, MoviesRepository>();
        return services;
    }
}
