using Blazored.LocalStorage;
using QuickFun.Domain.Entities;

namespace QuickFun.Infrastructure.Services;

public class LocalStorageGameSessionService : IGameSessionService
{
    private readonly ILocalStorageService _localStorage;
    private const string Key = "QuickFunSession";

    public LocalStorageGameSessionService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task AddGameResultAsync(GameResult result)
    {
        var session = await _localStorage.GetItemAsync<PlayerSession>(Key) ?? new PlayerSession();
        session.History.Add(result);
        await _localStorage.SetItemAsync(Key, session);
    }

    public async Task<string> GetPlayerNameAsync()
    {
        var session = await _localStorage.GetItemAsync<PlayerSession>(Key);
        return session?.PlayerName ?? "Anonim";
    }
    public async Task<List<GameResult>> GetSessionHistoryAsync()
    {
        var session = await _localStorage.GetItemAsync<PlayerSession>(Key);
        return session?.History ?? new List<GameResult>();
    }
    public async Task SavePlayerNameAsync(string name)
    {
        var session = await _localStorage.GetItemAsync<PlayerSession>(Key) ?? new PlayerSession();
        session.PlayerName = name;
        await _localStorage.SetItemAsync(Key, session);
    }
}