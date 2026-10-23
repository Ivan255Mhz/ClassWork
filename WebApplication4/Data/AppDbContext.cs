using Microsoft.EntityFrameworkCore;
using WebApplication4.Entities;

namespace WebApplication4.Data;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {}

    public DbSet<Toy> Toys => Set<Toy>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Manufacture> Manufactures => Set<Manufacture>();

}

