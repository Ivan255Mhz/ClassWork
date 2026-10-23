namespace WebApplication4.Data.Repositories;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync(CancellationToken ct = default);
    Task<T?> GetByIdAsync(int id ,CancellationToken ct = default);
    Task AddAsync(T entity, CancellationToken ct =default);
    void Remove(T entity);
    Task<int> SaveChagesAsync(CancellationToken ct =default);
}
