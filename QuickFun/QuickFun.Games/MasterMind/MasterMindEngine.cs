using System;
using System.Linq;
using QuickFun.Domain.Enums;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickFun.Games.Base;
using QuickFun.Games.MasterMind.Strategies;

namespace QuickFun.Games.Engines.MasterMind;

public class MasterMindEngine : BaseGameEngine
{
    private IMasterMindDifficultyStrategy? _strategy;

    public override GameType Type => GameType.MasterMind;
    public override string Name => "MasterMind";

    public int Score { get; private set; } = 0;
    public string SecretCode { get; private set; } = string.Empty;
    public List<(string Guess, int CorrectPosition, int CorrectColor)> GuessHistory { get; private set; } = new();
    public string Message { get; private set; } = "Choose a difficulty to start!";
    public bool IsGameOver { get; private set; } = false;
    public int MaxAttempts => _strategy?.MaxAttempts ?? 10;
    public int CodeLength => _strategy?.CodeLength ?? 4;
    public int Colors => _strategy?.Colors ?? 6;

    public event Action<int>? OnGameFinished;

    public async Task Start(Func<Task> onStateChanged)
    {
        Score = 0;
        IsGameOver = false;
        GuessHistory.Clear();
        SecretCode = GenerateSecretCode();
        Message = "Guess the secret code!";
        await onStateChanged();
    }

    public void SetDifficulty(IMasterMindDifficultyStrategy strategy)
    {
        _strategy = strategy;
    }

    private string GenerateSecretCode()
    {
        var random = new Random();
        int length = _strategy?.CodeLength ?? 4;
        int colors = _strategy?.Colors ?? 6;
        return new string(Enumerable.Range(0, length)
            .Select(_ => (char)('A' + random.Next(colors)))
            .ToArray());
    }

    public async Task MakeGuess(string guess, Func<Task> onStateChanged)
    {
        if (IsGameOver || _strategy == null) return;

        guess = guess.ToUpperInvariant();

        int correctPosition = guess.Zip(SecretCode, (g, s) => g == s).Count(b => b);
        int correctColor = guess.GroupBy(c => c)
            .Sum(g => Math.Min(g.Count(), SecretCode.Count(s => s == g.Key))) - correctPosition;

        GuessHistory.Add((guess, correctPosition, correctColor));
        Score++;

        if (guess == SecretCode)
        {
            Message = $"You cracked the code in {Score} attempt{(Score == 1 ? "" : "s")}! Your score: {(11 - Score) * Colors}";
            IsGameOver = true;
            OnGameFinished?.Invoke((11 - Score) * Colors);
        }
        else if (Score >= MaxAttempts)
        {
            Message = $"Out of attempts! The code was: {SecretCode}";
            IsGameOver = true;
            OnGameFinished?.Invoke(0);
        }
        else
        {
            Message = $"Attempt {Score}/{MaxAttempts} — keep going!";
        }

        await onStateChanged();
    }

    public override void OnReset()
    {
        Score = 0;
        IsGameOver = false;
        GuessHistory.Clear();
        SecretCode = string.Empty;
        Message = "Choose a difficulty to start!";
    }
}
