using Microsoft.EntityFrameworkCore;
using WebApplication4.Data.Repositories;

namespace WebApplication4.Data;

public static class DependencyInjection
{
    /// <summary>
    /// Регистрирует контекст БД и универсальный репозиторий.
    /// </summary>
    public static IServiceCollection AddDataBase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("В appsettings.json не задана строка подключения Default");

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped(typeof(IRepository<>), typeof(PostgresRepository<>));

        return services;
    }
}
