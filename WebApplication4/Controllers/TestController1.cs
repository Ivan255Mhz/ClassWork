using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using WebApplication4.Data.Dtos;
using WebApplication4.Entities;
using WebApplication4.Filters;
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

    [Authorize]
    [ServiceFilter(typeof(RespHeadFilter))]
    [HttpGet]
    public async Task<IActionResult> GetAll()
        
    {
        var result = await _services.GetAllAsync();

        return Ok(result);
    }

    [ServiceFilter(typeof(ToyExFilter))]
    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        var ex = new ArithmeticException();
        throw ex;
    }


    [Authorize(Policy = "OnliItAdmin")]
    [ServiceFilter(typeof(LogFilter))]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var sw = Stopwatch.StartNew();
        var result =  await _services.GetByIdAsync(id);
        sw.Stop();

        Console.WriteLine($"GetById занял {sw.ElapsedMilliseconds} мс");

        if(result is null)
        {
            Console.WriteLine("Ноль");
        }
        return Ok(result);  
    }

    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateToyDto dto)
    {
        var sw = Stopwatch.StartNew();

        var result = await _services.CreateToy(dto);

        sw.Stop();

        Console.WriteLine($"Create занял {sw.ElapsedMilliseconds} мс");

        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] CreateToyDto dto)
    {
        var sw = Stopwatch.StartNew();

        await _services.Remove(dto);
        sw.Stop();

        Console.WriteLine($"Delete занял {sw.ElapsedMilliseconds} мс");
        return NoContent();
    }
}
