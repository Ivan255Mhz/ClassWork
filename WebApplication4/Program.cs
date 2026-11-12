
using System.Diagnostics;
using System.Text.Json.Serialization;
using WebApplication4.Data;
using WebApplication4.Filters;
using WebApplication4.Middleware;
using WebApplication4.Servise;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers(op=>
{
    op.Filters.Add<TimingActionFilter>();
    op.Filters.Add<AntiHackerFilter>();
});
builder.Services.AddRazorPages();
builder.Services.AddDataBase(builder.Configuration);
builder.Services.AddScoped<IToyServices, ToyService>();
builder.Services.AddScoped<IDbLogger, DbLogger>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ToyExFilter>();
builder.Services.AddScoped<RespHeadFilter>();
builder.Services.AddScoped<ApiKeyAuthFilter>();
builder.Services.AddScoped<LogFilter>();
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

















