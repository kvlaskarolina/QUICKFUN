using Blazored.LocalStorage;
using QuickFun.Application.Interfaces;
using QuickFun.Domain.Entities;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace QuickFun.Infrastructure.Services;

/*
Data flow!!!
kończy się gra i wywoływany jest event OnGameFinished(score). W memory.razor silnik subskrybuje ten event wiec gdy gra się skończy wywołane zostaje HandleSaveScore. Ponieważ nie chcemy aby frontend sam zapisywał dane do bazy danych, przekazuje tą odpowiedzialność do SessionService czyli klasy LocalStorageGameSessionService która dziedziczy po IGameSessionService. Tam jest metoda AddGameResultAsync, która wysyła requesta w formacie json pod wskazany adres czyli do endpointa. W Program.cs jest endpoint czekający właśnie na takiego requesta, przyjmuje go i przekazuje requesta, baze i usera do metody UpdateStatsAsync znajdującej się w klasie StatsService, tam jest logika wybierania max score i dodawania do Total score

Dear Karolina honey jeśli to czytasz to koniecznie pobierz SQLite Viewer w VSCode to pozwala ładnie i przyjemnie ogladac QuickFun.db 
*/
public class LocalStorageGameSessionService : IGameSessionService
{
    private readonly ILocalStorageService _localStorage;
    private const string Key = "QuickFunSession";
    private readonly HttpClient _http;

    public LocalStorageGameSessionService(ILocalStorageService localStorage, HttpClient http)
    {
        _localStorage = localStorage;
        _http = http;
    }

    public async Task AddGameResultAsync(GameResult result)
    {
        var session = await _localStorage.GetItemAsync<PlayerSession>(Key) ?? new PlayerSession();
        session.History.Add(result);
        await _localStorage.SetItemAsync(Key, session);

        var request = new GameResultRequest(result.Score, result.Type);

        try
        {
            //wyciagamy token z local storage przegladarki
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrEmpty(token))
            {
                // przyklejamy token do nagłówka zapytania HTTP
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                //jezeli sie nie udalo wyciagnac tokena to nie jestesmy zalogowani
                Console.WriteLine("Warning! There is no token. User might not be singed in");
            }


            var response = await _http.PostAsJsonAsync("api/stats/save", request); //wysylamy jsona pod wskazany adres

            if (response.IsSuccessStatusCode) //messages ktore mozna zobaczyc w konsoli na przegladarce w trakcie testowania np
            {
                Console.WriteLine("Sukces, wynik zapisany w bazie danych na serwerze");
            }
            else
            {
                Console.WriteLine($"nie udało sie zapisac w bd");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd połączenia z serwerem: {ex.Message}");
        }
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
