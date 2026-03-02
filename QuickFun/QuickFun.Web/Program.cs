using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using QuickFun.Application.Interfaces;
using QuickFun.Infrastructure.Services;
using QuickFun.Games;
using QuickFun.Web;
using QuickFun.Web.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using QuickFun.Web.Components.Views;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSingleton<IGameFactory, GameFactory>();
builder.Services.AddScoped<IGameSessionService, LocalStorageGameSessionService>();

builder.Services.AddScoped<LeaderboardViewModel>();
builder.Services.AddScoped<LoginViewModel>();
builder.Services.AddScoped<RegisterViewModel>();
builder.Services.AddScoped<HomeViewModel>();
builder.Services.AddScoped<HangmanViewModel>();
builder.Services.AddScoped<MinesweeperViewModel>();
builder.Services.AddScoped<MemoryViewModel>();
builder.Services.AddScoped<TicTacToeWithAIViewModel>();

builder.Services.AddScoped(sp => new HttpClient {
    BaseAddress = new Uri("http://localhost:5253/")
});

builder.Services.AddHttpClient("SudokuApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:3000/");
});

await builder.Build().RunAsync();