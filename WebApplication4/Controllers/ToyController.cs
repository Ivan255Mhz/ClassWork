using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Data.Repositories;
using WebApplication4.Entities;

namespace WebApplication4.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ToyController : ControllerBase
{
    private readonly IRepository<Toy>? _toyRepository;
    private readonly IRepository<Category>? _categoryRepository;
    private readonly IRepository<Manufacture>? _manufactureRepository;





    public ToyController(IRepository<Toy>? toyRepository,
                         IRepository<Category>? categoryRepository,
                         IRepository<Manufacture>? manufactureRepository)
    {
        _toyRepository = toyRepository;
        _categoryRepository = categoryRepository;
        _manufactureRepository = manufactureRepository;
    }


    



    [HttpGet]

    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        return Ok(await _toyRepository.GetAllAsync(ct));
    }


    [HttpPost("{name}/{prise}/{catId:int}/{manId:int}")]
    public async Task<IActionResult> Create(string name, 
                                            decimal prise, 
                                            int catId,
                                            int manId,
                                            CancellationToken ct)
    {

        var cat = await _categoryRepository.GetByIdAsync(catId, ct);
        if(cat is null)
        {
            return BadRequest( new { Error ="Нет такой категории" });
        }

        var man = await _manufactureRepository.GetByIdAsync(manId, ct);
        if (man is null)
        {
            return BadRequest(new { Error = "Нет такого производителя" });
        }

        var toy = new Toy
        {
            Name = name,
            Prise = prise,
            ManufactureId = manId,
            CategoryId = catId
        };

        await _toyRepository.AddAsync(toy, ct);
        await _toyRepository.SaveChagesAsync(ct);

        return Ok(toy);

    }





}
