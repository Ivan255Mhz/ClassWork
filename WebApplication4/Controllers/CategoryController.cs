using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Data.Repositories;
using WebApplication4.Entities;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IRepository<Category>? _repository;

        public CategoryController(IRepository<Category>? repository)
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
            var category = new Category
            {
                Name = name
            };
            await _repository.AddAsync(category, ct);
            await _repository.SaveChagesAsync(ct);
            return Ok(category);

        }

    }
}
