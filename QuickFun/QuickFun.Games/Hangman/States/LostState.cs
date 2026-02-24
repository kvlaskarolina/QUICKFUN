namespace QuickFun.Games.Hangman.States
{
    public class LostState : IHangmanState
    {
        public string StateName => "Lost";
        public bool CanMakeMove => false;
        public bool IsGameOver => true;

        public IHangmanState HandleMove(HangmanEngine context, char letter) {
            return this;
        }

        public string GetMessage(HangmanEngine context) {
            return $"You lost! The word was: {context.WordToGuess}. Your score is {context.Score}";
        }
    }
}
