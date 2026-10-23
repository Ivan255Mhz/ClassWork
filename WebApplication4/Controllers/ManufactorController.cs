using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using WebApplication4.Data.Repositories;
using WebApplication4.Entities;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufactorController : ControllerBase
    {

        private readonly IRepository<Manufacture>? _repositories;

        public ManufactorController(IRepository<Manufacture>? repositories)
        {
            _repositories = repositories;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return Ok(await _repositories.GetAllAsync(ct));
        }

        [HttpPost("{name}")]
        public async Task<IActionResult> Create(string name, CancellationToken ct)
        {
            var manufacture = new Manufacture
            {
                Name = name,
            };
            await _repositories.AddAsync(manufacture, ct);
            await _repositories.SaveChagesAsync(ct);

            return Ok(manufacture);

        }
    }
}
