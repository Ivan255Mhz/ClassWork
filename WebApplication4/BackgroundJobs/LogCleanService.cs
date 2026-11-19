using Microsoft.EntityFrameworkCore;
using WebApplication4.Data;

namespace WebApplication4.BackgroundJobs;

/// <summary>
/// Периодически удаляет логи старше 7 дней.
/// </summary>
public class LogCleanService : BackgroundService
{
    private static readonly TimeSpan CleanupInterval = TimeSpan.FromMinutes(1);
    private static readonly TimeSpan LogRetention = TimeSpan.FromDays(7);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<LogCleanService> _logger;
    private readonly WorkerHealthState _state;

    public LogCleanService(
        IServiceScopeFactory scopeFactory,
        ILogger<LogCleanService> logger,
        WorkerHealthState state)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _state = state;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Сервис очистки логов запущен");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var cutoffDate = DateTime.UtcNow - LogRetention;
                var oldLogs = await db.Logs
                    .Where(log => log.Timestamp < cutoffDate)
                    .ToListAsync(stoppingToken);

                if (oldLogs.Count > 0)
                {
                    db.Logs.RemoveRange(oldLogs);
                    await db.SaveChangesAsync(stoppingToken);

                    _logger.LogInformation("Удалено {Count} старых логов", oldLogs.Count);
                }

                _state.LastHeartbeat = DateTime.UtcNow;
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при очистке логов");
            }

            await Task.Delay(CleanupInterval, stoppingToken);
        }
    }
}
