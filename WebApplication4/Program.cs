using Microsoft.OpenApi;
using WebApplication4.Data;
using WebApplication4.Servise;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddDataBase(builder.Configuration);
builder.Services.AddScoped<IToyServices,ToyService>();
builder.Services.AddScoped<IDbLogger, DbLogger>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( op=>
{
    var xmlFile = "${ System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.xml}";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
});



builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
