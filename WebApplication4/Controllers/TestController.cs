using System.Threading.Channels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.BackgroundJobs;
using WebApplication4.Data.Dtos;
using WebApplication4.Filters;
using WebApplication4.Services;

namespace WebApplication4.Controllers;

/// <summary>
/// Учебный контроллер: демонстрация фильтров, фоновых задач и авторизации.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IToyService _toyService;

    public TestController(IToyService toyService)
    {
        _toyService = toyService;
    }

    /// <summary>
    /// Состояние фоновых задач.
    /// </summary>
    [HttpGet("state")]
    public IActionResult GetState([FromServices] WorkerHealthState state)
    {
        if (!state.IsAlive)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Фоновые задачи зависли");
        }

        return Ok("Все системы работают");
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromServices] Channel<string> emailQueue, CancellationToken ct)
    {
        var toys = await _toyService.GetAllAsync(ct);

        await emailQueue.Writer.WriteAsync("Промо-письмо для получения игрушек", ct);

        return Ok(toys);
    }

    [ServiceFilter(typeof(ToyExFilter))]
    [HttpGet("test")]
    public IActionResult Test()
    {
        throw new ArithmeticException("Тестовое исключение");
    }

    [Authorize(Policy = "OnlyAdmin")]
    [ServiceFilter(typeof(LogFilter))]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var toy = await _toyService.GetByIdAsync(id, ct);

        return toy is null
            ? NotFound($"Игрушка с id {id} не найдена")
            : Ok(toy);
    }

    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateToyDto dto, CancellationToken ct)
    {
        var toy = await _toyService.CreateToy(dto, ct);

        return toy is null
            ? BadRequest("Категория или производитель не найдены")
            : Ok(toy);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var removed = await _toyService.RemoveAsync(id, ct);

        return removed ? NoContent() : NotFound($"Игрушка с id {id} не найдена");
    }
}
