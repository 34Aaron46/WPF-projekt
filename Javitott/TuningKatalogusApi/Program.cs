using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TuningKatalogusApi.Data;

var builder = WebApplication.CreateBuilder(args);

// a wpf ezen a cimen eri el az api-t
builder.WebHost.UseUrls("http://localhost:5080");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// az adatbazis elerese az appsettings fajlbol jon
var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySQL(   connStr));

var app = builder.Build();

// indulaskor lefuttatja a migraciot es letrehozza a tablakat meg a kezdo adatokat
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// globalis hibakezeles
// a hibat kiirja a naplobe a kliens csak egy rovid uzenetet kap
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