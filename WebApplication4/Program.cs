
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;
using WebApplication4.Data;
using WebApplication4.Filters;
using WebApplication4.Helpers;
using WebApplication4.Middleware;
using WebApplication4.Servise;

var builder = WebApplication.CreateBuilder(args);

//401 Unauthorized - Аутенификация код 
//403 Forbidden -  Авторизация код 


builder.Services.AddAuthentication("Custom")
    .AddScheme<AuthenticationSchemeOptions, MyHandler>("Custom", null);

builder.Services.AddAuthorization(op =>
{
    op.AddPolicy("OnliItAdmin", policy =>
    {
        policy.RequireRole("Admin");
        policy.RequireClaim("Deportament", "IT");
    });
});


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

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.UseMiddleware<ServerTimeMiddleware>();
app.UseMiddleware<AuditBodyMiddlewere>();
app.MapControllers();
app.MapRazorPages();

app.Run();

















