using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication4.Data;
using WebApplication4.Data.Repositories;
using WebApplication4.Entities;
using WebApplication4.Pages;

namespace WebApplication4.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ToyController : ControllerBase
{
    private readonly IRepository<Toy>? _toyRepository;
    private readonly IRepository<Category>? _categoryRepository;
    private readonly IRepository<Manufacture>? _manufactureRepository;

    private readonly AppDbContext _appDbContext;





    public ToyController(IRepository<Toy>? toyRepository,
                         IRepository<Category>? categoryRepository,
                         IRepository<Manufacture>? manufactureRepository,
                         AppDbContext appDbContext)
    {
        _toyRepository = toyRepository;
        _categoryRepository = categoryRepository;
        _manufactureRepository = manufactureRepository;
        _appDbContext = appDbContext;
    }



    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id , CancellationToken ct)
    {
        var toy = await _toyRepository.GetByIdAsync(id, ct);

        return toy is null ? NotFound() : Ok(toy);
    }


    // url/api/toy?name=Машинка&prise=50

    [HttpPost]
    public async Task<IActionResult> AddToy([FromQuery]string? name,
                                            [FromQuery]decimal? prise,CancellationToken ct)
    {
        if (name is null || prise is null) return BadRequest();

        var toy = new Toy { Name=name,Prise=(decimal)prise,CategoryId=1,ManufactureId=1 };
        await _toyRepository.AddAsync(toy, ct);
        await _toyRepository.SaveChagesAsync(ct);
        return Ok(toy);
    }




    //[HttpGet]
    //public async Task<IActionResult> GetByCatIdEndManId
    //    (
    //    [FromQuery] int? cat,
    //    [FromQuery] int? man,
    //    CancellationToken ct
    //    )
    //{
    //    var toyList = await _toyRepository.GetAllAsync(ct);

    //    if( toyList is null ) return NotFound();

    //    var res = toyList.Where(x=> x.ManufactureId==man && x.CategoryId==cat);

    //    if( !res.Any()) return NotFound();

    //    return Ok(res);

    //}



    //[HttpGet("{id:int}")]

    //public async Task<IActionResult> GetById([FromRoute]int id , CancellationToken ct)
    //{
    //    var toy = _toyRepository.GetByIdAsync(id,ct);

    //    if (toy is null) return BadRequest();
    //    return Ok(toy);

    //}

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        return Ok( await _appDbContext.Toys.Include(c=>c.Category).ThenInclude(c=>c.Toys).Include(m=>m.Manufacture).ThenInclude(m=>m.Toys).ToListAsync());
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
