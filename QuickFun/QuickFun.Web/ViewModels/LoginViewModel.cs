using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using QuickFun.Domain.Entities;

namespace QuickFun.Web.ViewModels;

public class LoginViewModel
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _nav;

    public LoginViewModel(HttpClient http, ILocalStorageService localStorage, NavigationManager nav) {
        _http = http; _localStorage = localStorage; _nav = nav;
    }

    public string email = "";
    public string password = "";
    public string errorMessage = "";

    [Parameter]
    [SupplyParameterFromQuery(Name = "returnUrl")]
    public string? ReturnUrl { get; set; }

    private class AuthResponse
    {
        public string accessToken { get; set; } = "";
    }

    public async Task HandleLogin()
    {
        //wysylamy zapytanie json do endpointu /api/accountlogin ktory sami zrobilismy
        var response = await _http.PostAsJsonAsync("/api/account/login", new { email = email, password = password });
        if (response.IsSuccessStatusCode)
        {
            //dostajemy od serwera w odpowiedzi obiekt ktory w srodku ma specjalny access token
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            
            if (result != null && !string.IsNullOrEmpty(result.accessToken))
            {
                //narazie token jest w pamieci ram, ale ram sie kasuje za kazdym odswiezeniem strony
                //dlatego chowamy token do local storage przelgadarki
                //czat mowi ze "LocalStorage to taki mały dysk twardy wewnątrz Twojej przeglądarki."
                await _localStorage.SetItemAsync("authToken", result.accessToken);

                // Jeśli wiemy skąd przyszedł użytkownik (ReturnUrl), odsyłamy go tam.
                // W przeciwnym razie idziemy na stronę główną "/".
                string destination = !string.IsNullOrEmpty(ReturnUrl) ? ReturnUrl : "/";
                Console.WriteLine($"Przekierowuję do: {destination}");
                
                //przechodzimy do strony głownej po udanym zalogowaniu
                _nav.NavigateTo(destination, forceLoad: true);
            }
        }
        else
        {
            // Czytamy co dokładnie odpowiedział serwer
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"BŁĄD LOGOWANIA ({response.StatusCode}): {errorContent}");
            
            errorMessage = response.StatusCode switch
            {
            System.Net.HttpStatusCode.Unauthorized => "Invalid email or password. Please try again.",
            System.Net.HttpStatusCode.BadRequest => "The login request was invalid.",
            System.Net.HttpStatusCode.InternalServerError => "Server error. Please try again later.",
            _ => "An unexpected error occurred. Please check your connection."
            };
        }
    }
}