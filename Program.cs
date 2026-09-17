var builder = WebApplication.CreateBuilder(args);

// Добавляем MVC (контроллеры + представления). TempData (cookie-based) подключается автоматически.
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Middleware, которое отдаёт статические файлы из wwwroot (css, js, img, lib/bootstrap)
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// По умолчанию открываем каталог товаров
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Catalog}/{action=Index}/{id?}");

app.Run();
