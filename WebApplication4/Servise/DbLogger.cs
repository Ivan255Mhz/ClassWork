using WebApplication4.Data;
using WebApplication4.Entities;

namespace WebApplication4.Servise;

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
            Detalis = details ?? ""
        };

        try
        {
            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Неудачная запись в db {ex.Message} , {ex.StackTrace}");
        }

    }
}

public static class LogLevel
{
    public const string Info = "[INFO]";
    public const string Error = "[ERROR]";
    public const string Worning = "[WORN]";
}
