using Microsoft.AspNetCore.Mvc;
using WebApplication4.Data.Dtos;
using WebApplication4.Services;

namespace WebApplication4.Controllers;

/// <summary>
/// Контроллер игрушек.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ToyController : ControllerBase
{
    private readonly IToyService _toyService;
    private readonly ILogger<ToyController> _logger;
    private readonly IDbLogger _dbLogger;
    private readonly IFileStorege _fileStorege;

    public ToyController(IToyService toyService, ILogger<ToyController> logger, IDbLogger dbLogger, IFileStorege fileStorege)
    {
        _toyService = toyService;
        _logger = logger;
        _dbLogger = dbLogger;
        _fileStorege = fileStorege;
    }

    /// <summary>
    /// Получить список всех игрушек.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        _logger.LogInformation("Запрошен список всех игрушек");

        return Ok(await _toyService.GetAllAsync(ct));
    }

    /// <summary>
    /// Получить игрушку по id.
    /// </summary>
    /// <param name="id">Id игрушки в БД.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var toy = await _toyService.GetByIdAsync(id, ct);

        if (toy is null)
        {
            _logger.LogWarning("Запрошена несуществующая игрушка с id {Id}", id);
            return NotFound($"Игрушка с id {id} не найдена");
        }

        return Ok(toy);
    }

    /// <summary>
    /// Создать игрушку.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateToyDto dto, CancellationToken ct)
    {
        await _dbLogger.LogAsync(DbLogLevel.Info, "Начало создания игрушки");

        var toy = await _toyService.CreateToy(dto, ct);

        if (toy is null)
        {
            await _dbLogger.LogAsync(DbLogLevel.Warning, "Ошибка создания игрушки");
            return BadRequest("Категория или производитель не найдены");
        }

        await _dbLogger.LogAsync(DbLogLevel.Info, $"Игрушка успешно создана c id {toy.Id}");

        return Ok(toy);
    }

    [HttpPost("{id:int}/image")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<IActionResult> UploudImage(int id, IFormFile? file, CancellationToken ct)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Выбирите файл");

        var toy = await _toyService.GetByIdAsync(id, ct);

        if (toy is null)
            return NotFound($" Игрушка с id {id} не найдена");

        var oldImageUrl = toy.ImageUrl;

        string imageUrl;
        try
        {
            imageUrl = await _fileStorege.SaveToyImageAsync(file, ct);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        toy.ImageUrl = imageUrl;
        await _toyService.UpdateAsync(toy,ct);

        await _fileStorege.DeleteAsync(oldImageUrl, ct);

        return Ok(new { imageUrl });
    }

    /// <summary>
    /// Тестовый endpoint для проверки обработки ошибок.
    /// </summary>
    [HttpGet("error")]
    public IActionResult TriggerError()
    {
        throw new InvalidOperationException("Это тестовая ошибка");
    }
}
