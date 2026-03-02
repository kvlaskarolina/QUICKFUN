using System.Net.Http.Json;
using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace QuickFun.Web.ViewModels;

public class HomeViewModel
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public HomeViewModel(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    public string? Nickname { get; set; }

    public async Task InitializeAsync()
    {
        Nickname = null;

        //czy w "schowku" jest token
        var token = await _localStorage.GetItemAsync<string>("authToken");

        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                //tworzymy zapytanie i dodajemy token do niego
                var request = new HttpRequestMessage(HttpMethod.Get, "api/account/me");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                var response = await _http.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var info = await response.Content.ReadFromJsonAsync<UserInfo>();
                    Nickname = info?.Username;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not find profile: {ex.Message}");
            }
        }
    }
    
    //klasa pomocnicza do odebrania jsona z serwera
    private class UserInfo { public string Username { get; set; } = ""; }
}