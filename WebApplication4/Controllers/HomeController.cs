using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Data;

namespace WebApplication4.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeController : ControllerBase
{
    /// <summary>
    /// Проверка доступности БД.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get([FromServices] AppDbContext db)
    {
        return Ok(await db.Database.CanConnectAsync());
    }
}
