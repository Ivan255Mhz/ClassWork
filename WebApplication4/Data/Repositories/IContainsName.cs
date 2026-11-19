namespace WebApplication4.Data.Repositories;

/// <summary>
/// Репозиторий с поиском по названию.
/// </summary>
public interface IContainsName<T>
{
    Task<T?> ContainsNameAsync(string name, CancellationToken ct = default);
}
