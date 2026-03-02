using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using QuickFun.Domain.Entities;


namespace QuickFun.Web.ViewModels;

public class RegisterViewModel
{
    private readonly HttpClient _http;
    private readonly NavigationManager _nav;

    public RegisterViewModel(HttpClient http, NavigationManager nav)
    {
        _http = http;
        _nav = nav;
    }

    public string email { get; set; } = "";
    public string username { get; set; } = "";
    public string password { get; set; } = "";
    public string errorMessage { get; set; } = "";
    public bool isSuccess { get; set; } = false;
    public async Task HandleRegister()
    {
        errorMessage = "";
        isSuccess = false;

        // Walidacja na froncie
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(username))
        {
            errorMessage = "Please fill in all fields!";
            return;
        }

        // Nowa paczka danych
        var registerData = new
        {
            Email = email,
            Username = username,
            Password = password
        };

        try
        {
            // Wysyłamy paczkę pod nowy endpoint
            var response = await _http.PostAsJsonAsync("api/account/register", registerData);

            if (response.IsSuccessStatusCode)
            {
                isSuccess = true;
                email = "";
                username = "";
                password = "";
            }
            else
            {
                // Próbujemy odczytać błędy z serwera
                var errors = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
                
                if (errors != null && errors.Any())
                {
                    // Identity API zazwyczaj zwraca błędy po angielsku domyślnie, więc tu wystarczy join
                    errorMessage = string.Join(", ", errors);
                }
                else
                {
                    errorMessage = "Registration failed.";
                }
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Connection error: {ex.Message}";
        }
    }
}