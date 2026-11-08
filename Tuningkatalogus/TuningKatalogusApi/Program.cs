using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TuningKatalogusApi.Data;

var builder = WebApplication.CreateBuilder(args);

// a WPF alkalmazás ezen a címen éri el az API-t
builder.WebHost.UseUrls("http://localhost:5080");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TuningContext>(options => options.UseMySQL(connStr));

var app = builder.Build();

// indításkor lefuttatjuk a migrációkat, így létrejönnek a táblák és a kezdő adatok
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TuningContext>();
    db.Database.Migrate();
}

// globális hibakezelés: a hibát naplózzuk, a kliens csak egy egyszerű üzenetet kap
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var hiba = context.Features.Get<IExceptionHandlerFeature>();
        if (hiba != null)
        {
            app.Logger.LogError(hiba.Error, "Kezeletlen hiba történt");
        }
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { uzenet = "Szerverhiba történt!" });
    });
});

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
