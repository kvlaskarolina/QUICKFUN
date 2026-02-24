namespace QuickFun.Games.Hangman.States
{
    public class WonState : IHangmanState
    {
        public string StateName => "Won";
        public bool CanMakeMove => false;
        public bool IsGameOver => true;

        public IHangmanState HandleMove(HangmanEngine context, char letter) {
            return this;
        }

        public string GetMessage(HangmanEngine context) {
            return $"You guessed the word '{context.WordToGuess}'! Your score is {context.Score}";
        }
    }
}
