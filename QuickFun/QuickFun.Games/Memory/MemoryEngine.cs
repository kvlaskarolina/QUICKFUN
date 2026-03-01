using System;
using System.Linq;
using QuickFun.Domain.Enums;
using QuickFun.Games.Memory.Strategies;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickFun.Games.Base;
namespace QuickFun.Games.Memory;


public class MemoryCard
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsRevealed { get; set; } = false;
    public bool IsMatched { get; set; } = false;
}

public class MemoryEngine : BaseGameEngine

{
    private IMemoryDifficultyStrategy? _strategy;
    public int Columns => _strategy?.Columns ?? 4;
    public override GameType Type => GameType.Memory;
    public override string Name => "Memory";
    public int Score { get; private set; } = 0;
    public List<MemoryCard> Cards { get; private set; } = new();
    public string Message { get; private set; } = "Choose difficulty";
    public bool IsGameOver { get; private set; } = false;
    private CancellationTokenSource? _startCts;
    public event Action<int>? OnGameFinished; //potrzebne do api
    public MemoryEngine()
    {
        ResetState();

    }
    private void ResetState()
    {
        _startCts?.Cancel();
        _startCts = null;

        Score = 0;
        IsGameOver = false;
        _isBusy = false;
        _firstCard = null;
        _secondCard = null;
    }

    public void SetDifficulty(IMemoryDifficultyStrategy strategy)
    {
        _strategy = strategy;
        Reset();
    }

    public void Reset()
    {
        if (_strategy == null) return;
        ResetState();
        Message = "Find pairs";
        InitializeGame();
    }

    public void InitializeGame()
    {
        var paths = new List<string>
        {
            "/images/imbir.png", "/images/jablka.png", "/images/lody.png", "/images/nos.png",
            "/images/pies.png", "/images/piwo.png", "/images/superman.png", "/images/truskawka.png",
            "/images/watermelon.png", "/images/eggplant.png", "/images/plankton.png", "/images/squidward.png",
            "/images/bravo.png",  "/images/tree.png",  "/images/gpt.png",  "/images/messi.png",
            "/images/fcb.png",  "/images/super.png",
        };

        int totalCards = _strategy.Rows * _strategy.Columns;
        int pairsNeeded = totalCards / 2;

        var selectedPaths = paths.Take(pairsNeeded).ToList();
        var pathsPairs = selectedPaths.Concat(selectedPaths).OrderBy(x => Guid.NewGuid()).ToList();

        this.Cards = pathsPairs.Select((path, index) => new MemoryCard
        {
            Id = index,
            ImageUrl = path,
            IsRevealed = false,
            IsMatched = false
        }).ToList();
    }


    private MemoryCard? _firstCard;
    private MemoryCard? _secondCard;
    private bool _isBusy = false;

    public async Task Start(Func<Task> onStateChanged)
    {
        if (_strategy == null) return;

        _startCts?.Cancel();
        _startCts = new CancellationTokenSource();
        var token = _startCts.Token;

        try
        {
            _isBusy = true;
            Message = $"Remember the arrangement!";

            foreach (var card in Cards) card.IsRevealed = true;
            await onStateChanged();

            await Task.Delay(_strategy.DelayMs, token);

            foreach (var card in Cards) card.IsRevealed = false;
            Message = "Let's start! Find all pairs";
            _isBusy = false;
            await onStateChanged();
        }
        catch (TaskCanceledException)
        {
            // metoda została przerwana, bo wybrano nowy poziom - nic nie robimy
        }
        finally
        {
            if (_startCts?.Token == token)
            {
                _isBusy = false;
            }
        }
    }

    public async Task HandleCardClick(MemoryCard clicked)
    {
        if (_isBusy || clicked.IsRevealed || clicked.IsMatched || IsGameOver) return;
        clicked.IsRevealed = true;
        if (_firstCard == null)
        {
            _firstCard = clicked;
            Message = "Choose second card";
        }
        else
        {
            _secondCard = clicked;
            _isBusy = true;
            await CheckMatch();
        }
    }

    private async Task CheckMatch()
    {
        if (_firstCard!.ImageUrl == _secondCard!.ImageUrl)
        {
            _firstCard.IsMatched = true;
            _secondCard.IsMatched = true;
            if (_strategy != null)
            {
                Score += _strategy.ScorePerRound;
            }
            else
            {
                Score += 5;
            }
            Message = "Pair found!";
        }
        else
        {
            Message = "It does not match!";
            Score -= 2;
            await Task.Delay(1000);
            _firstCard.IsRevealed = false;
            _secondCard.IsRevealed = false;
        }

        _firstCard = null;
        _secondCard = null;
        _isBusy = false;

        CheckWin();
    }
    public override void OnReset()
    {
        Reset();
    }

    private void CheckWin()
    {
        if (Cards.All(card => card.IsMatched))
        {
            IsGameOver = true;
            Message = $"Huge Win!!! Your score is {Score}!";

            OnGameFinished?.Invoke(Score);
        }
    }
}