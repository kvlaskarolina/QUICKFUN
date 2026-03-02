using Microsoft.AspNetCore.Components;
using QuickFun.Application.Interfaces;
using QuickFun.Domain.Entities;
using QuickFun.Domain.Enums;
using QuickFun.Games.Engines.TicTacToe.AI;
using QuickFun.Domain.Enums;
using QuickFun.Games.TicTacToe.Strategies;

namespace QuickFun.Web.ViewModels;

public class TicTacToeWithAIViewModel : IDisposable
{
    private readonly IGameSessionService _sessionService;
    
    public TicTacToeEngineWithAI? Engine { get; private set; }
    public Level SelectedLevel { get; set; } = Level.Medium;
    public event Action? OnNotifyView;

    public TicTacToeWithAIViewModel(IGameSessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public void SetEngine(TicTacToeEngineWithAI engine)
    {
        if (Engine != null)
        {
            Engine.OnGameFinished -= HandleSaveScore;
        }

        Engine = engine;
        Engine.OnGameFinished += HandleSaveScore;
    }

    public async Task MakeMove(int index)
    {
        if (Engine == null) return;
        await Engine.MakeMove(index);
        Notify();
    }

    public void UndoMove()
    {
        Engine?.Undo();
        Notify();
    }

    public void SetDifficulty(Level level)
    {
        SelectedLevel = level;
        Engine?.SetDifficulty(level);
        Engine?.ResetGame();
        Notify();
    }

    private async void HandleSaveScore(int finalScore)
    {
        if (Engine == null) return;

        var result = new GameResult
        {
            Type = Engine.Type,
            Score = finalScore
        };

        await _sessionService.AddGameResultAsync(result);
        Notify();
    }

    private void Notify() => OnNotifyView?.Invoke();

    public void Dispose()
    {
        if (Engine != null)
            Engine.OnGameFinished -= HandleSaveScore;
    }
}