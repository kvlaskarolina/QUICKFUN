using QuickFun.Games.TicTacToe.Strategies;
using QuickFun.Domain.Enums;
using QuickFun.Games.Engines;

namespace QuickFun.Games.Engines.TicTacToe.AI
{

    public class TicTacToeEngineWithAI : TicTacToeEngine
    {
        private ITicTacToeDifficultyStrategy _aiStrategy;
        private readonly CommandHistory _history = new CommandHistory();

        public TicTacToeEngineWithAI(ITicTacToeDifficultyStrategy strategy)
        {
            _aiStrategy = strategy;
            Score = 20;
        }

        public TicTacToeEngineWithAI()
        {
            _aiStrategy = new TicTacToeMediumStrategy();
            Score = 20;
        }

        public new void Reset()
        {
            base.Reset();
            _history.Clear();
            if (_aiStrategy == null) _aiStrategy = new TicTacToeMediumStrategy();
            Score = 20;
        }

        public void ResetGame() => Reset();

        public void Undo()
        {
            if (IsGameOver) IsGameOver = false;

            _history.UndoLast();
            _history.UndoLast();

            CurrentPlayer = 'X';
            Message = "Undo performed.";
            Score -= 5;
        }

        public void SetDifficulty(Level level)
        {
            _aiStrategy = level switch
            {
                Level.Easy => new TicTacToeEasyStrategy(),
                Level.Medium => new TicTacToeMediumStrategy(),
                Level.Hard => new TicTacToeHardStrategy(),
                _ => new TicTacToeMediumStrategy()
            };
        }

        public new async Task MakeMove(int index)
        {
            if (IsGameOver || index < 0 || index >= Board.Length || Board[index] != '\0')
                return;

            // Ruch Gracza przez Command
            var playerCmd = new MoveCommand(Board, index, CurrentPlayer, () => { });
            Score -= 1;
            _history.ExecuteCommand(playerCmd);

            if (CheckWinner())
            {
                Message = "You WON!";
                IsGameOver = true;
                return;
            }
            if (IsBoardFull()) { Message = "DRAW!"; IsGameOver = true; Score = 0; return; }

            CurrentPlayer = 'O';
            await Task.Delay(500);

            int aiMove = MakeAIMove();
            if (aiMove != -1)
            {
                var aiCmd = new MoveCommand(Board, aiMove, CurrentPlayer, () => { });
                _history.ExecuteCommand(aiCmd);

                if (CheckWinner()) { Message = "TAKE THE L LOOSER!"; IsGameOver = true; Score -= 30; return; }
                if (IsBoardFull()) { Message = "DRAW!"; IsGameOver = true; Score = 0; return; }

                CurrentPlayer = 'X';
            }
        }

        private int MakeAIMove()
        {
            int[][] boardForAI = ConvertBoardForAI();
            return _aiStrategy.MakeAIMove(boardForAI);
        }

        private int[][] ConvertBoardForAI()
        {
            int[][] board2D = new int[3][];
            for (int i = 0; i < 3; i++)
            {
                board2D[i] = new int[3];
                for (int j = 0; j < 3; j++)
                {
                    int index = i * 3 + j;
                    board2D[i][j] = Board[index] switch
                    {
                        '\0' => 0,
                        'X' => 1,
                        'O' => 2,
                        _ => 0
                    };
                }
            }
            return board2D;
        }

        private bool IsBoardFull() => Board.All(c => c != '\0');
    }
}