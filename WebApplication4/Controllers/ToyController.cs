using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
public class ToyController : ControllerBase
{

    private readonly ToyService _services;
    

    public ToyController(ToyService? services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok( await _services.GetAllAsync());
    }
    

}


    

   




