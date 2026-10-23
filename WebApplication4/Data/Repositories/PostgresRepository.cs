

using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Data.Repositories;

public class PostgresRepository<T> : IRepository<T>  where T : class
{
    private readonly AppDbContext? _context;
    
    public PostgresRepository(AppDbContext? context)
    {
        _context = context;
    }


    public async Task AddAsync(T entity, CancellationToken ct = default)
    {
        await _context.Set<T>().AddAsync(entity,ct);
    }

    public  async Task<List<T>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync(ct);
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Set<T>().FindAsync( new object[] { id }, ct);
    }

    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public async Task<int> SaveChagesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}
