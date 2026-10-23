using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Data.Repositories;

namespace WebApplication4.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDataBase(this IServiceCollection services, 
                                                    IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped(typeof(IRepository<>),typeof(PostgresRepository<>));

        return services;

    }
}

