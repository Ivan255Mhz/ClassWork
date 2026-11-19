namespace WebApplication4.Services;

public interface IFileStorege
{
    Task<string> SaveToyImageAsync(IFormFile file, CancellationToken ct = default);
    Task DeleteAsync(string? ImageUrl,CancellationToken ct =default);
}
