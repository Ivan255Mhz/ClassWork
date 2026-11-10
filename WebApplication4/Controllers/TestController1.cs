using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApplication4.Data.Dtos;
using WebApplication4.Entities;
using WebApplication4.Servise;

namespace WebApplication4.Controllers;

// реализовать 4 endpoints получить все игрушки , получить по айди , добавить , удалить 

// задача от заказчика в ответе получат ьвремя сервера по ключу X-Server-Time 

// логировать тело пост запроса в бд 

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IToyServices _services;
    private readonly IDbLogger _dbLogger;

    public TestController(IToyServices services, IDbLogger dbLogger)
    {
        _services = services;
        _dbLogger = dbLogger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _services.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {

        var result = _services.GetByIdAsync(id);
        return Ok(result);  
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateToyDto dto)
    {
        return Ok(await _services.CreateToy(dto));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] CreateToyDto dto)
    {
        await _services.Remove(dto);
        return NoContent();
    }
}
