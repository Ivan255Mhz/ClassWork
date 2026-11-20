using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebApplication4.BackgroundJobs;
using WebApplication4.Data;
using WebApplication4.Endpoints;
using WebApplication4.Filters;
using WebApplication4.Middleware;
using WebApplication4.Services;

var builder = WebApplication.CreateBuilder(args);

// --- Аутентификация и авторизация (JWT) ---
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("В appsettings.json не задан Jwt:Key");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // проверка того, кто выдал токен
            ValidateAudience = false, // проверка того, для кого выдан токен
            ValidateLifetime = true, // проверка срока действия
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OnlyAdmin", policy => policy.RequireRole("Admin"));
});

// --- MVC, Razor Pages, Swagger ---
builder.Services.AddControllers(options =>
    {
        options.Filters.Add<TimingActionFilter>();
        options.Filters.Add<AntiHackerFilter>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- База данных и репозитории ---
builder.Services.AddDataBase(builder.Configuration);

// --- Сервисы приложения ---
builder.Services.AddScoped<IToyService, ToyService>();
builder.Services.AddScoped<IDbLogger, DbLogger>();
builder.Services.AddScoped<IFileStorege, LocalFileStorage>();

// --- Фильтры ---
builder.Services.AddScoped<ToyExFilter>();
builder.Services.AddScoped<RespHeadFilter>();
builder.Services.AddScoped<ApiKeyAuthFilter>();
builder.Services.AddScoped<LogFilter>();

// --- Фоновые задачи ---
builder.Services.AddSingleton<WorkerHealthState>();
builder.Services.AddSingleton<Channel<string>>(_ => Channel.CreateUnbounded<string>());
builder.Services.AddHostedService<LogCleanService>();
builder.Services.AddHostedService<EmailWorker>();

// --- События жизненного цикла приложения ---
builder.Services.AddSingleton<AppLifetimeService>();

var app = builder.Build();

// Singleton создаётся лениво: запрашиваем его, чтобы он подписался на события жизненного цикла.
app.Services.GetRequiredService<AppLifetimeService>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ServerTimeMiddleware>();
app.UseMiddleware<AuditBodyMiddleware>();
app.UseMiddleware<UserMiddleware>();

app.MapControllers();
app.MapToyEndpoints();
app.MapRazorPages();

app.Run();
