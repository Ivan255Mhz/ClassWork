using WebApplication4.Data.Dtos;
using WebApplication4.Entities;

namespace WebApplication4.Services;

/// <summary>
/// Бизнес-логика работы с игрушками.
/// </summary>
public interface IToyService
{
    Task<Toy?> CreateToy(CreateToyDto dto, CancellationToken ct = default);

    Task<Toy?> UpdateToy(CreateToyDto dto, CancellationToken ct = default);

    Task<List<Toy>> GetAllAsync(CancellationToken ct = default);

    Task<Toy?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<bool> RemoveAsync(int id, CancellationToken ct = default);

    Task UpdateAsync(Toy toy, CancellationToken ct = default);
}
