using Microsoft.AspNetCore.Components;
using QuickFun.Application.Interfaces;
using QuickFun.Domain.Entities;
using QuickFun.Games.Hangman;

namespace QuickFun.Web.ViewModels;

public class HangmanViewModel : IDisposable
{
    private readonly IGameSessionService _sessionService;
    public HangmanEngine? Engine { get; private set; }
    public char[] Letters { get; } = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

    public HangmanViewModel(IGameSessionService sessionService)
    {
        _sessionService = sessionService;
    }

    //metoda inicjalizująca wywoływana przez view, gdy dostanie Engine jako parametr
    public void SetEngine(HangmanEngine engine)
    {
        if (Engine != null) // odpinamy się od starego silnika jeśli był
            Engine.OnGameFinished -= HandleSaveScore;

        Engine = engine;
        Engine.OnGameFinished += HandleSaveScore; //i dopinamy z powrotem
    }

    public HashSet<char> GetUsedLetters()
    {
        return Engine?.GuessedLetters != null
            ? new HashSet<char>(Engine.GuessedLetters)
            : new HashSet<char>();
    }

    public void StartGame()
    {
        Engine?.Reset();
    }

    public void HandleGuess(char letter)
    {
        if (Engine == null || Engine.IsGameOver) return;
        Engine.MakeMove(letter);
    }

    private async void HandleSaveScore(int finalScore)
    {
        if (Engine == null) return;

        var result = new GameResult
        {
            Type = Engine.Type,
            Score = finalScore
        };

        await _sessionService.AddGameResultAsync(result); //serwis wysyla to do api
    }

    public void Dispose()
    {
        if (Engine != null)
            Engine.OnGameFinished -= HandleSaveScore;
    }
}