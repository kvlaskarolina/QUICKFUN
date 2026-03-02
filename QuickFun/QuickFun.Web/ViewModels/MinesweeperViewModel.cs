using QuickFun.Application.Interfaces;
using QuickFun.Domain.Entities;
using QuickFun.Games.Minesweeper;
using Microsoft.AspNetCore.Components;

namespace QuickFun.Web.ViewModels;

public class MinesweeperViewModel : IDisposable
{
    private readonly IGameSessionService _sessionService;
    public MinesweeperEngine? Engine { get; private set; }
    public event Action? OnNotifyView;

    public MinesweeperViewModel(IGameSessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public void SetEngine(MinesweeperEngine engine)
    {
        if (Engine != null)
        {
            Engine.OnStateChanged -= Notify;
            Engine.OnGameFinished -= HandleSaveScore;
        }

        Engine = engine;
        Engine.OnStateChanged += Notify;
        Engine.OnGameFinished += HandleSaveScore;
    }

    private void Notify() => OnNotifyView?.Invoke();

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

    public void Dispose()
    {
        if (Engine != null)
        {
            Engine.OnStateChanged -= Notify;
            Engine.OnGameFinished -= HandleSaveScore;
        }
    }
}