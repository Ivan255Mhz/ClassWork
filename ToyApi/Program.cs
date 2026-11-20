using Microsoft.EntityFrameworkCore;
using ToyApi.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("В appsettings.json не задана строка подключения Default");

builder.Services.AddDbContext<ToyDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Небольшая проверка, что приложение запустилось.
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

// Проверка, что EF действительно видит базу.
app.MapGet("/health/db", async (ToyDbContext db, CancellationToken ct) =>
    Results.Ok(new { canConnect = await db.Database.CanConnectAsync(ct) }));

// TODO: здесь студенты добавляют endpoint'ы Minimal API.
//
// Пример:
// app.MapGet("/api/toys", async (ToyDbContext db, CancellationToken ct) =>
// {
//     var toys = await db.Toys
//         .AsNoTracking()
//         .ToListAsync(ct);
//
//     return Results.Ok(toys);
// });

app.Run();
