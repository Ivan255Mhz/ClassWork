using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using WebApplication4.Data;
using WebApplication4.Data.Dtos;
using WebApplication4.Data.Repositories;
using WebApplication4.Entities;
using WebApplication4.Pages;
using WebApplication4.Servise;

namespace WebApplication4.Controllers;

[Route("api/[controller]")]
[ApiController]


///<summary>
///Контролер игрушек
///</summary>
public class ToyController : ControllerBase
{

    private readonly IToyServices _services;

    private readonly ILogger<ToyController> _logger;

    private readonly IDbLogger _dbLogger;

    public ToyController(IToyServices services, ILogger<ToyController> logger, IDbLogger dbLogger)
    {
        _services = services;
        _logger = logger;
        _dbLogger = dbLogger;
    }


    /// <summary>
    /// Метод получения всех игрушек 
    /// </summary>
    /// <param 
    /// <returns></returns>

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("Запрошен список всех игрушек");
        return Ok(await _services.GetAllAsync());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateToyDto dto, CancellationToken ct)
    {
        await _dbLogger.LogAsync(Servise.LogLevel.Info,"Начало создания игрушки");

        var toy =  await _services.CreateToy(dto, ct);

        if (toy is null) 
        {
            await _dbLogger.LogAsync(Servise.LogLevel.Worning, "Ошибка создания игрушки");
            return BadRequest("Ошибка создания игрушки ");
        }

        await _dbLogger.LogAsync(Servise.LogLevel.Info, $"Игрушка успешно создана  c id {toy.Id}"); 

        return Ok(toy);

    }




    /// <summary>
    /// Метод получения игрушки по ID 
    /// </summary>
    /// <param name="id"> id  игрушки в БД</param>
    /// <returns>Ок+ json игрушки </returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) 
    {
        var toy = await _services.GetByIdAsync(id);
        if (toy is null) 
        {
            _logger.LogWarning($"Запрошена несуществующая игрушка с ID {id}");
            return BadRequest("Нету игрушек");
        }
        
        return Ok(toy);
    }
    

}


    

   




