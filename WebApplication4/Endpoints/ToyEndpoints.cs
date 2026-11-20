using Microsoft.IdentityModel.Tokens;
using WebApplication4.Data.Dtos;
using WebApplication4.Services;

namespace WebApplication4.Endpoints;

public static class ToyEndpoints
{
    public static IEndpointRouteBuilder MapToyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/mini/toys").WithTags("Minamal API:  игрушки");

        group.MapGet("/", async (IToyService service , CancellationToken ct) =>
        {
            var toys =await service.GetAllAsync(ct);

            return  Results.Ok(toys);
        });

        group.MapGet("/{id:int}",async (int id , IToyService servise, CancellationToken ct) =>
        {
            var  toy = await servise.GetByIdAsync(id,ct);

            return toy is null ? Results.NotFound("Bla bla bla") : Results.Ok(toy);
        });


        group.MapPost("/", async (CreateToyDto dto, IToyService servise, CancellationToken ct) =>
        {
            if(string.IsNullOrWhiteSpace(dto.Name)|| dto.Prise <=0)
            {
                return Results.ValidationProblem(
                    new Dictionary<string, string[]>
                    {
                        ["name"] = new[] {"Нахвание обеательно"},
                        ["prise"] = new[] {"Цена у нас должна быть больше 0"} 
                    });
            }

            var  toy =  await servise.CreateToy(dto,ct);

            if (toy is null)
                return Results.BadRequest("Bla bla bla bla ");

            return Results.Created($"/api/mini/toys/{toy.Id}", toy);

        });

        group.MapDelete("/{id:int}", async (int id, IToyService servise, CancellationToken ct) =>
        {
            var remoded = await servise.RemoveAsync(id,ct);

            return remoded ? Results.NoContent() : Results.NotFound("bla bla bla bla ");
        });

        return app;
    }
}
