namespace QuickFun.Games.Engines.TicTacToe.AI
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    public class MoveCommand : ICommand
    {
        private readonly char[] _board;
        private readonly int _index;
        private readonly char _player;
        private readonly Action _onChanged;

        public MoveCommand(char[] board, int index, char player, Action onChanged)
        {
            _board = board;
            _index = index;
            _player = player;
            _onChanged = onChanged;
        }

        public void Execute()
        {
            _board[_index] = _player;
            _onChanged?.Invoke();
        }

        public void Undo()
        {
            _board[_index] = '\0';
            _onChanged?.Invoke();
        }

        public void Restart()
        {
            for (int i = 0; i < _board.Length; i++)
                _board[i] = '\0';
            _onChanged?.Invoke();
        }
    }
}