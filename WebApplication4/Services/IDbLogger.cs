namespace WebApplication4.Services;

/// <summary>
/// Абстракция над записью логов в хранилище.
/// </summary>
public interface IDbLogger
{
    Task LogAsync(string level, string message, string? details = null);
}
