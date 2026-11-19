using Microsoft.EntityFrameworkCore;
using WebApplication4.Data;
using WebApplication4.Entities;

namespace WebApplication4.Services;

/// <summary>
/// Пишет записи журнала в базу данных.
/// </summary>
public class DbLogger : IDbLogger
{
    private readonly AppDbContext _context;
    private readonly ILogger<DbLogger> _logger;

    public DbLogger(AppDbContext context, ILogger<DbLogger> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task LogAsync(string level, string message, string? details = null)
    {
        var log = new LogEntry
        {
            Timestamp = DateTime.UtcNow,
            Level = level,
            Message = message,
            Detalis = details ?? string.Empty
        };

        try
        {
            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Убираем неудачную запись из ChangeTracker,
            // иначе она будет ломать все следующие SaveChanges в этом запросе.
            _context.Entry(log).State = EntityState.Detached;

            _logger.LogError(ex, "Не удалось записать лог в БД");
        }
    }
}

/// <summary>
/// Уровни логирования для записей в БД.
/// </summary>
public static class DbLogLevel
{
    public const string Info = "[INFO]";
    public const string Error = "[ERROR]";
    public const string Warning = "[WARN]";
}
