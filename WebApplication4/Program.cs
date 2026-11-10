
using System.Diagnostics;
using System.Text.Json.Serialization;
using WebApplication4.Data;
using WebApplication4.Middleware;
using WebApplication4.Servise;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddDataBase(builder.Configuration);
builder.Services.AddScoped<IToyServices, ToyService>();
builder.Services.AddScoped<IDbLogger, DbLogger>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });



var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();  

app.UseAuthorization();
app.UseMiddleware<ServerTimeMiddleware>();
app.UseMiddleware<AuditBodyMiddlewere>();
app.MapControllers();
app.MapRazorPages();

app.Run();

















