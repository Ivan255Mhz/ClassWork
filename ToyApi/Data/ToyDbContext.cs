using Microsoft.EntityFrameworkCore;
using ToyApi.Entities;

namespace ToyApi.Data;

/// <summary>
/// Контекст Minimal API-проекта. Смотрит в ту же БД, что и WebApplication4.
/// Миграции здесь не создаём: схемой владеет основной проект.
/// </summary>
public class ToyDbContext : DbContext
{
    public ToyDbContext(DbContextOptions<ToyDbContext> options)
        : base(options)
    {
    }

    public DbSet<Toy> Toys => Set<Toy>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Manufacture> Manufactures => Set<Manufacture>();
}
