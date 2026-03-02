using Microsoft.AspNetCore.Components;
using QuickFun.Application.Interfaces;
using QuickFun.Domain.Entities;
using QuickFun.Games.Memory;
using QuickFun.Games.Memory.Strategies;

namespace QuickFun.Web.ViewModels;

public class MemoryViewModel : IDisposable
{
    private readonly IGameSessionService _sessionService;
    public MemoryEngine? Engine { get; private set; }
    public bool IsDifficultySelected { get; set; } = false;

    public MemoryViewModel(IGameSessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public void Initialize(MemoryEngine engine)
    {
        if (Engine != null)
        {
            Engine.OnGameFinished -= HandleSaveScore;
        }

        Engine = engine;
        Engine.OnGameFinished += HandleSaveScore;
    }

    public async Task SelectDifficulty(IMemoryDifficultyStrategy strategy, Func<Task> onStateChanged)
    {
        if (Engine == null) return;
        
        IsDifficultySelected = true;
        Engine.SetDifficulty(strategy);

        //uruchamiamy silnik, przekazując mu funkcję odświeżania UI
        await Engine.Start(onStateChanged);
    }

    public void ChangeLevel()
    {
        IsDifficultySelected = false;
    }

    public async Task HandleCardClick(MemoryCard card)
    {
        if (Engine == null) return;
        await Engine.HandleCardClick(card);
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
    }

    public void Dispose()
    {
        if (Engine != null)
        {
            Engine.OnGameFinished -= HandleSaveScore;
        }
    }
}