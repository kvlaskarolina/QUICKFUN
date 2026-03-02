using QuickFun.Domain.Enums;
using QuickFun.Domain.Entities;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace QuickFun.Web.ViewModels;

public class LeaderboardViewModel
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public LeaderboardViewModel(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    public GameType selectedGame = GameType.TicTacToeWithAI;
    public List<LeaderboardDto>? topScores;
    public UserStatDto? myCurrentGameStat;
    public bool isLoggedIn = false;

    public async Task ChangeGame(GameType type) {
        selectedGame = type;
        await RefreshData();
    }

    public async Task RefreshData() {
        // Zawsze czyścimy stare statystyki przed pobraniem nowych, 
        // żeby nie pokazywać przez sekundę wyniku z poprzedniej gry
        myCurrentGameStat = null;

        // 1. Pobierz Global Top 5 (Dostępne dla każdego)
        topScores = await _http.GetFromJsonAsync<List<LeaderboardDto>>($"api/stats/leaderboard/{selectedGame}");

        // 2. Obsługa logowania i statystyk prywatnych
        var token = await _localStorage.GetItemAsync<string>("authToken");
        isLoggedIn = !string.IsNullOrEmpty(token);

        if (isLoggedIn) {
            try {
                var request = new HttpRequestMessage(HttpMethod.Get, "api/stats/my-stats");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                var response = await _http.SendAsync(request);
                
                if (response.IsSuccessStatusCode) {
                    var allMyStats = await response.Content.ReadFromJsonAsync<List<UserStatDto>>();
                    // Szukamy rekordu dla obecnie wybranej gry
                    myCurrentGameStat = allMyStats?.FirstOrDefault(x => x.GameName == selectedGame.ToString());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                    // Token wygasł w trakcie przeglądania
                    isLoggedIn = false;
                    await _localStorage.RemoveItemAsync("authToken");
                }
            }
            catch {
                isLoggedIn = false;
            }
        }
    }
}