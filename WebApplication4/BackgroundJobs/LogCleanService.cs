using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using WebApplication4.Data;

namespace WebApplication4.BackgroundJobs;

public class LogCleanService : BackgroundService
{

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly ILogger<LogCleanService> _logger;

    public LogCleanService(IServiceScopeFactory scopeFactory, ILogger<LogCleanService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Сервис очистки логов запущен");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Начали очистку логов");
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var CutDate = DateTime.UtcNow.AddDays(-7);
                    var oldLogs = await db.Logs.Where(l => l.Timestamp < CutDate).ToListAsync(stoppingToken);

                    if (oldLogs.Any())
                    {
                        db.Logs.RemoveRange(oldLogs);
                        await db.SaveChangesAsync();

                        _logger.LogInformation($"удалено {oldLogs.Count} старых логов");
                    }
                    else
                    {
                        _logger.LogInformation("Старых логов не найдено");
                    }
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Ошибка при очистке логов");
            }

            await Task.Delay(TimeSpan.FromMinutes(1),stoppingToken);
        }
    }
}
