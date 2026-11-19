namespace WebApplication4.Servise;

public interface IDbLogger
{
    Task LogAsync(string level, string message, string? details = null);
}
