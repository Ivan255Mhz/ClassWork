using WebApplication4.Data.Dtos;
using WebApplication4.Data.Repositories;
using WebApplication4.Entities;

namespace WebApplication4.Services;

/// <summary>
/// Реализация бизнес-логики работы с игрушками.
/// </summary>
public class ToyService : IToyService
{
    private readonly IRepository<Toy> _toyRepository;
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<Manufacture> _manufactureRepository;

    public ToyService(
        IRepository<Toy> toyRepository,
        IRepository<Category> categoryRepository,
        IRepository<Manufacture> manufactureRepository)
    {
        _toyRepository = toyRepository;
        _categoryRepository = categoryRepository;
        _manufactureRepository = manufactureRepository;
    }

    public async Task<Toy?> CreateToy(CreateToyDto dto, CancellationToken ct = default)
    {
        if (!await CheckConnectionsAsync(dto, ct))
        {
            return null;
        }

        var toy = new Toy
        {
            Name = dto.Name,
            Prise = dto.Prise,
            CategoryId = dto.CategoryId,
            ManufactureId = dto.ManufacturerId
        };

        await _toyRepository.AddAsync(toy, ct);
        await _toyRepository.SaveChangesAsync(ct);

        return toy;
    }

    public Task<Toy?> UpdateToy(CreateToyDto dto, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<Toy>> GetAllAsync(CancellationToken ct = default)
    {
        return _toyRepository.GetAllAsync(ct);
    }

    public Task<Toy?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return _toyRepository.GetByIdAsync(id, ct);
    }

    public async Task<bool> RemoveAsync(int id, CancellationToken ct = default)
    {
        var toy = await _toyRepository.GetByIdAsync(id, ct);

        if (toy is null)
        {
            return false;
        }

        _toyRepository.Remove(toy);
        await _toyRepository.SaveChangesAsync(ct);

        return true;
    }

    private async Task<bool> CheckConnectionsAsync(CreateToyDto dto, CancellationToken ct = default)
    {
        var category = await _categoryRepository.GetByIdAsync(dto.CategoryId, ct);
        var manufacture = await _manufactureRepository.GetByIdAsync(dto.ManufacturerId, ct);

        return category is not null && manufacture is not null;
    }

    public async Task UpdateAsync(Toy toy, CancellationToken ct = default)
    {
        _toyRepository.Update(toy);
        await _toyRepository.SaveChangesAsync(ct);
    }
}
