namespace WebApplication4.Data.Repositories;

public interface IConteinName<T>
{
    Task<T> ConteinNameAsync(string name,CancellationToken ct);
}
