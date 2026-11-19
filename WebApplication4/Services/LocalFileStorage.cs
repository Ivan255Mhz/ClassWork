using System.Data;

namespace WebApplication4.Services;

public class LocalFileStorage : IFileStorege
{

    private const long _maxFileSize = 5 * 1024 * 1024; // 5 МБ

    private static readonly HashSet<string> _allowedExtensions =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

    private readonly IWebHostEnvironment _environment;

    public LocalFileStorage(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public Task DeleteAsync(string? ImageUrl, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(ImageUrl))
            return Task.CompletedTask;

        var fileName = Path.GetFileName(ImageUrl);
        var fullPatc = Path.Combine(GetUploadFolder(), fileName);

        if (File.Exists(fullPatc))
            File.Delete(fullPatc);
        return Task.CompletedTask;
    }

    public async Task<string> SaveToyImageAsync(IFormFile file, CancellationToken ct = default)
    {
        if (file.Length == 0)
            throw new ArgumentException("Файл пустой");
        if (file.Length > _maxFileSize)
            throw new ArgumentException("Файл больше 5 МБ");

        var extension = Path.GetExtension(file.FileName);

        if (!_allowedExtensions.Contains(extension))
            throw new ArgumentException("Неверный формат");

        if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Файл не похож на изображение");

        var foldet = GetUploadFolder();

        var fileName = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
        var filePath = Path.Combine(foldet, fileName);

        await using var stream = new FileStream(filePath, FileMode.CreateNew);
        await file.CopyToAsync(stream, ct);

        return $"/uploads/toys/{fileName}";

    }

    private string GetUploadFolder()
    {
        var webRoot = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");

        var folder = Path.Combine(webRoot, "uploads", "toys");

        Directory.CreateDirectory(folder);

        return folder;
    }
}
