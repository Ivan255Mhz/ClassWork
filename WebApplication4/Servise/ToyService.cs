using WebApplication4.Data.Dtos;
using WebApplication4.Data.Repositories;
using WebApplication4.Entities;

namespace WebApplication4.Servise;

public class ToyService : IToyServices
{

    private readonly IRepository<Toy>? _toyRepository;
    private readonly IRepository<Category>? _categoryRepository;
    private readonly IRepository<Manufacture>? _manufactureRepository;
    private readonly ILogger<IToyServices>? _logger;

    public ToyService(IRepository<Toy>? toyRepository, IRepository<Category>? categoryRepository, IRepository<Manufacture>? manufactureRepository, ILogger<IToyServices>? logger)
    {
        _toyRepository = toyRepository;
        _categoryRepository = categoryRepository;
        _manufactureRepository = manufactureRepository;
        _logger = logger;
    }

    public async Task<Toy> CreateToy(CreateToyDto dto, CancellationToken ct = default)
    {
        if( await СheckingСonnections(dto,ct))
        {
            var toy = new Toy
            {
                Name = dto.Name,
                Prise = dto.Prise,
                CategoryId = dto.CategoryId,
                ManufactureId = dto.ManufacturerId,
            };

            await _toyRepository.AddAsync(toy, ct);
            await _toyRepository.SaveChagesAsync(ct);
            return toy;
        }

        return null;
    }

    public async Task<Toy> UpdateToy(CreateToyDto dto, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    private async Task<bool> СheckingСonnections(CreateToyDto dto,CancellationToken ct = default)
    {
        var cat = await _categoryRepository.GetByIdAsync(dto.CategoryId, ct);
        var man = await _manufactureRepository.GetByIdAsync(dto.ManufacturerId, ct);
       

        if (cat is null || man is null) return false;
        return true;

    }

    public async Task<List<Toy>> GetAllAsync(CancellationToken ct = default)
    {
        return await _toyRepository.GetAllAsync(ct);
    }

    public async Task<Toy> TaskAsync(CreateToyDto dto,CancellationToken ct)
    {
        var toy = new Toy
        {
            Name = dto.Name,
            Prise = dto.Prise,
            CategoryId = dto.CategoryId,
            ManufactureId = dto.ManufacturerId,
        };

        await _toyRepository.AddAsync(toy, ct);
        await _toyRepository.SaveChagesAsync(ct);
        return toy;
    }

    public async Task<Toy> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var toy = await _toyRepository.GetByIdAsync(id, ct);
        if (toy is null) return null;

        return toy;
    }


    public  async Task Remove(CreateToyDto dto)
    {
        var toy = new Toy
        {
            Name = dto.Name,
            Prise = dto.Prise,
            CategoryId = dto.CategoryId,
            ManufactureId = dto.ManufacturerId,
        };

        _toyRepository?.Remove(toy);
        await _toyRepository?.SaveChagesAsync();
        
    }

}
