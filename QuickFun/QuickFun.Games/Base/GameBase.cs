using QuickFun.Domain.Enums;
using QuickFun.Games;

namespace QuickFun.Games.Base
{
    public abstract class BaseGameEngine : IGameEngine
    {
        public abstract GameType Type { get; }
        public abstract string Name { get; }
        public int Score { get; protected set; } = 0;
        public bool IsGameOver { get; protected set; } = false;
        public string Message { get; protected set; } = string.Empty;

        public void Reset()
        {
            System.Console.WriteLine($"Resetting {Name}...");
            ResetState();
            OnReset();
        }

        protected virtual void ResetState()
        {
            Score = 0;
            IsGameOver = false;
            Message = string.Empty;
        }

        public abstract void OnReset();
    }
}