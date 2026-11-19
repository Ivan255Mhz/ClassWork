using Microsoft.AspNetCore.Mvc;
using WebApplication4.Data.Repositories;
using WebApplication4.Entities;

namespace WebApplication4.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ManufacturerController : ControllerBase
{
    private readonly IRepository<Manufacture> _repository;

    public ManufacturerController(IRepository<Manufacture> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        return Ok(await _repository.GetAllAsync(ct));
    }

    [HttpPost("{name}")]
    public async Task<IActionResult> Create(string name, CancellationToken ct)
    {
        var manufacture = new Manufacture
        {
            Name = name
        };

        await _repository.AddAsync(manufacture, ct);
        await _repository.SaveChangesAsync(ct);

        return Ok(manufacture);
    }
}
