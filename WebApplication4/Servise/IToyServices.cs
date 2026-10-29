using System.Runtime.CompilerServices;
using WebApplication4.Data.Dtos;
using WebApplication4.Entities;

namespace WebApplication4.Servise;

public interface IToyServices 
{
    Task<Toy> CreateToy(CreateToyDto dto, CancellationToken ct = default);
    Task<Toy> UpdateToy(CreateToyDto dto, CancellationToken ct = default);
    Task<List<Toy>> GetAllAsync(CancellationToken ct = default);
    Task<Toy> GetByIdAsync(int id, CancellationToken ct = default);
}
