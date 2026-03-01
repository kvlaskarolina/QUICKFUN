using QuickFun.Domain.Enums;
using QuickFun.Games.Hangman.States;
using QuickFun.Games.Hangman.WordProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using QuickFun.Games.Base;

namespace QuickFun.Games.Hangman
{
    public class HangmanEngine : BaseGameEngine
    {
        public override GameType Type => GameType.Hangman;
        public override string Name => "Hangman";
        public int Score { get; set; } = 0;

        public string WordToGuess { get; private set; } = string.Empty;
        public char[] CurrentGuess { get; set; } = Array.Empty<char>();
        public int WrongAttempts { get; set; }
        public int MaxAttempts { get; private set; }
        public HangmanDifficulty Difficulty { get; private set; }

        private IHangmanState _currentState;
        public IHangmanState CurrentState
        {
            get => _currentState;
            private set
            {
                _currentState = value;
                Message = _currentState.GetMessage(this);
            }
        }

        public bool IsGameOver => CurrentState.IsGameOver;
        public string Message { get; private set; } = string.Empty;

        private List<char> _guessedLetters = new List<char>();
        public IReadOnlyCollection<char> GuessedLetters => _guessedLetters;

        private readonly IWordProvider _wordProvider;

        public event Action<int>? OnGameFinished; //do api

        public HangmanEngine(IWordProvider wordProvider, HangmanDifficulty difficulty)
        {
            _wordProvider = wordProvider;
            Difficulty = difficulty;

            MaxAttempts = difficulty switch
            {
                HangmanDifficulty.Easy => 10,
                HangmanDifficulty.Medium => 7,
                HangmanDifficulty.Hard => 5,
                _ => 7
            };

            _currentState = new NotStartedState();
        }
        public override void OnReset()
        {
            Reset();
        }
        public void Reset()
        {
            WordToGuess = _wordProvider.GetRandomWord();
            CurrentGuess = new string('_', WordToGuess.Length).ToCharArray();
            WrongAttempts = 0;
            _guessedLetters.Clear();
            Score = 0;

            CurrentState = new PlayingState();
        }

        public void MakeMove(char letter)
        {
            if (!CurrentState.CanMakeMove) return;

            if (_guessedLetters.Contains(letter)) return;

            _guessedLetters.Add(letter);

            CurrentState = CurrentState.HandleMove(this, letter);

            if (CurrentState.IsGameOver)
            {
                OnGameFinished?.Invoke(Score);
            }
        }

        public string HangmanPicture => HangmanPictures.GetPicture(Difficulty, WrongAttempts);

        public string GetCurrentStateName() => CurrentState.StateName;
    }
}