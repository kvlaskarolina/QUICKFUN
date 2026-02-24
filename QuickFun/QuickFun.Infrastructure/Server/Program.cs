using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuickFun.Infrastructure.Data;
using System.Security.Claims;
using QuickFun.Infrastructure.Services;
using QuickFun.Domain.Enums;
using QuickFun.Domain.Entities;

//wzorzec builder wow
var builder = WebApplication.CreateBuilder(args);


//TESTOWANIE LOKALNIE
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

//swagger czyli dokumentacja API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//glowny manager db to klasa ApplicationDbContext. silnikiem db jest SqlLite
//tutaj tworzymy obiekt options z ktorego korzystamy w konstruktorze w ApplicationDbContext.cs
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=../QuickFun.db"));

//dodajemy stats service potrzebne do liczenia punktow
builder.Services.AddScoped<QuickFun.Infrastructure.Services.StatsService>();

//autoryzacja, czyli sprawdzanie uprawnien
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder czyli szablon zamienia sie na w app czyli gotowa instancje aplikacji webowej
var app = builder.Build();

//TESTOWANIE LOKALNIE
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//szalona opcja, jedna linijka która tworzy potrzebne do logowania endpointy
app.MapIdentityApi<IdentityUser>();

//endpoint do zapisywania wynikow w bazie
//swoją drogą, korzystamy tu z minimal API ktore jest alternatywa do full API (czyli do kontrolerów - tych z modelu ModelVievController!!!)
//klasyczne full api wymaga od nas zdefiniowania atrybutu, ustawienia adresu url, konstruktora itd
//minimal api jest prosztą i wydajniejszą wersją endpointu (ma swoje wady ofc takie jak brak porządku)

//z dokumentacji "Route handlers are methods that execute when the route matches. Route handlers can be a lambda expression, a local function,
//an instance method or a static method. Route handlers can be synchronous or asynchronous.
//u nas jest to lambda expression, w parametrach prosimy o request (dane z gry), statsService (logike dk bazy danych) i usera.

app.MapPost("/api/stats/save", async (GameResultRequest request, StatsService statsService, ClaimsPrincipal user) =>
{
    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

    if (string.IsNullOrEmpty(userId))
    {
        return Results.Unauthorized();
    }

    //var userId = "9976e510-0e1b-46b2-a70d-6770a03442ee"; do testowania

    await statsService.UpdateStatsAsync(userId, request.Score, request.GameType);

    return Results.Ok(new { message = "Score saved"});
}
).RequireAuthorization();


app.Run();