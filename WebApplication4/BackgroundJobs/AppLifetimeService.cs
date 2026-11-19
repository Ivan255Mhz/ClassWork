using WebApplication4.Data;
using WebApplication4.Entities;

namespace WebApplication4.BackgroundJobs;

/// <summary>
/// Пишет события запуска и остановки приложения в таблицу LifeEvent.
/// </summary>
public class AppLifetimeService
{
    private readonly ILogger<AppLifetimeService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public AppLifetimeService(
        IHostApplicationLifetime applicationLifetime,
        IServiceScopeFactory scopeFactory,
        ILogger<AppLifetimeService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;

        applicationLifetime.ApplicationStarted.Register(() => WriteEvent("Started"));
        applicationLifetime.ApplicationStopping.Register(() => WriteEvent("Stopping"));
        applicationLifetime.ApplicationStopped.Register(() => WriteEvent("Stopped"));
    }

    private void WriteEvent(string eventType)
    {
        _logger.LogInformation("Событие жизненного цикла приложения: {EventType}", eventType);

        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.Events.Add(new AppLifeEvent { EventType = eventType });
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не удалось сохранить событие {EventType}", eventType);
        }
    }
}
