
using System.Diagnostics;
using WebApplication4.Data;
using WebApplication4.Middleware;
using WebApplication4.Servise;


var builder = WebApplication.CreateBuilder(args);
// Содание зависемостей 
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddDataBase(builder.Configuration);
builder.Services.AddScoped<IToyServices, ToyService>();
builder.Services.AddScoped<IDbLogger, DbLogger>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(op =>
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


//-PipeLine

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Use(async (context, next) =>
{

    //---------Код До контроллера----------------
    var stopwatch = Stopwatch.StartNew();
    Console.WriteLine($"[START] Пришел запрос: {context.Request.Method}, {context.Request.Path}");
    //---------Код До контроллера----------------

    await next();

    //--------Код ПОСЛЕ контроллера--------------
    stopwatch.Stop();
    Console.WriteLine($"[END] Запрос обработан за {stopwatch.ElapsedMilliseconds} мс. Статус:{context.Response.StatusCode}");

});

app.UseMiddleware<ClientVersionMiddlewere>();
app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseMiddleware<TimingHeaderMiddleware>();
app.UseMiddleware<ResWrapMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();  // |

app.UseAuthorization(); // |

app.MapControllers();
app.MapRazorPages();

app.Run();





//app.Use(async (context, next) =>
//{
//    if (context.Request.Path.StartsWithSegments("/api"))
//    {
//        if (!context.Request.Headers.ContainsKey("X-Api-Key"))
//        {
//            context.Response.StatusCode = 401;
//            await context.Response.WriteAsync("Ошибка: Заголовок X-Api-Key обезателен!");
//            return;
//        }
//    }

//    await next();
//});

//app.UseWhen(
//    context => context.Request.Path.StartsWithSegments("/api/toy/error"),
//    appBuilder =>
//    {

//    appBuilder.Use(async (context, next) =>
//    {
//        if (context.Request.Path.StartsWithSegments("/api"))
//        {
//            if (!context.Request.Headers.ContainsKey("X-Api-Key"))
//            {
//                context.Response.StatusCode = 401;
//                await context.Response.WriteAsync("Ошибка: Заголовок X-Api-Key обезателен!");
//                return;
//            }
//        }

//        await next();
//    });



//    });











