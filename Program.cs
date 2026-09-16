using System.Diagnostics;
using TaskBoard.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Шаг 3.2: регистрация сервиса задач в DI.
// Singleton, потому что данные хранятся в памяти процесса на всё время жизни
// приложения — при Scoped/Transient каждый запрос получал бы свой список
// задач и данные "терялись" бы между запросами. При перезапуске приложения
// (dotnet run заново) данные всё равно будут потеряны, т.к. хранилище — не БД.
builder.Services.AddSingleton<ITaskService, InMemoryTaskService>();

var app = builder.Build();

// ---- Middleware pipeline ----

// Шаг 2: замер времени обработки запроса.
// Стоит ДО логирующего middleware (ближе к началу конвейера), чтобы замерить
// время выполнения ВСЕХ последующих middleware и самого обработчика запроса,
// включая логирование. Если поставить его после логирующего middleware, то
// в замер не попадёт время работы самого логирования (хотя разница обычно
// пренебрежимо мала) — здесь важен сам принцип "оборачивания" конвейера.
app.Use(async (context, next) =>
{
    var sw = Stopwatch.StartNew();
    // Регистрируем колбэк ДО вызова next(): если добавлять заголовок уже
    // после await next(), к этому моменту ответ мог начать отправляться
    // (например, если следующий middleware сам делает WriteAsync, как
    // middleware для /health) — тогда заголовки уже "read-only" и попытка
    // их изменить бросает исключение. OnStarting гарантированно срабатывает
    // непосредственно перед отправкой заголовков, поэтому это безопасно.
    context.Response.OnStarting(() =>
    {
        context.Response.Headers.Append("X-Response-Time-ms", sw.ElapsedMilliseconds.ToString());
        return Task.CompletedTask;
    });
    await next(context);
    sw.Stop();
});

// Шаг 1: заголовок и логирование запросов.
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-App-Name", "TaskBoard");
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("--> {Method} {Path}", context.Request.Method, context.Request.Path);
    await next(context);
    logger.LogInformation("<-- {StatusCode}", context.Response.StatusCode);
});

// Шаг 1 (усложнение): короткое замыкание для /health.
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/health")
    {
        context.Response.StatusCode = 200;
        await context.Response.WriteAsync("healthy");
        return; // не вызываем next — запрос не идёт дальше
    }
    await next(context);
});

// Уровень 2: проверка X-Api-Key для всех запросов к /tasks/api/*.
const string ApiKey = "secret123";
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/tasks/api"))
    {
        if (!context.Request.Headers.TryGetValue("X-Api-Key", out var providedKey) ||
            providedKey != ApiKey)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: missing or invalid X-Api-Key header");
            return;
        }
    }
    await next(context);
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
