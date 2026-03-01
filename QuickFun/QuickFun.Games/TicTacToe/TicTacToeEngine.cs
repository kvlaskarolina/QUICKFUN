using QuickFun.Domain.Enums;
using QuickFun.Games.Base;
using System.Linq;
namespace QuickFun.Games.Engines.TicTacToe
{
    public class TicTacToeEngine : BaseGameEngine
    {
        public override GameType Type => GameType.TicTacToe;
        public override string Name => "TicTacToe";
        public int Score { get; protected set; } = 10;
        public char[] Board { get; protected set; } = new char[9];
        public char CurrentPlayer { get; protected set; } = 'X';
        public bool IsGameOver { get; protected set; }
        public string Message { get; protected set; } = "X starts";

        public TicTacToeEngine()
        {
            OnReset();
        }

        void OnReset()
        {
            Board = new char[9];
            CurrentPlayer = 'X';
            IsGameOver = false;
            Score = 10;
            Message = "X starts";
        }

        public void MakeMove(int index)
        {
            if (IsGameOver || Board[index] != '\0') return;

            Board[index] = CurrentPlayer;
            Score -= 1;

            if (CheckWinner())
            {
                Message = $"{CurrentPlayer} WON!";
                IsGameOver = true;
            }
            else if (Board.All(c => c != '\0'))
            {
                Message = "DRAW!";
                IsGameOver = true;
                Score = 0;
            }
            else
            {
                CurrentPlayer = (CurrentPlayer == 'X') ? 'O' : 'X';
                Message = $"{CurrentPlayer}'s turn";
            }
        }

        protected bool CheckWinner()
        {
            int[][] winners = new int[][]
            {
                new[] {0,1,2}, new[] {3,4,5}, new[] {6,7,8}, // Poziomo
                new[] {0,3,6}, new[] {1,4,7}, new[] {2,5,8}, // Pionowo
                new[] {0,4,8}, new[] {2,4,6}             // Skosy
            };

            foreach (var w in winners)
            {
                if (Board[w[0]] != '\0' &&
                    Board[w[0]] == Board[w[1]] &&
                    Board[w[0]] == Board[w[2]])
                {
                    return true;
                }
            }
            return false;
        }
    }
}