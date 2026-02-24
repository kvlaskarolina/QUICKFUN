using System.Linq;

namespace QuickFun.Games.Hangman.States
{
    public class PlayingState : IHangmanState
    {
        public string StateName => "Playing";
        public bool CanMakeMove => true;
        public bool IsGameOver => false;

        public IHangmanState HandleMove(HangmanEngine context, char letter)
        {
            if (context.WordToGuess.Contains(letter))
            {
                int occ = context.WordToGuess.Count(c => c == letter);
                context.Score += occ * 5;

                for (int i = 0; i < context.WordToGuess.Length; i++)
                {
                    if (context.WordToGuess[i] == letter)
                    {
                        context.CurrentGuess[i] = letter;
                    }
                }

                if (!context.CurrentGuess.Contains('_'))
                {
                    int attemptsLeft = context.MaxAttempts - context.WrongAttempts;
                    context.Score += (attemptsLeft * 5);
                    
                    return new WonState();
                }

                return this;
            }
            else
            {
                context.WrongAttempts++;

                if (context.WrongAttempts >= context.MaxAttempts)
                {
                    return new LostState();
                }

                return this;
            }
        }

        public string GetMessage(HangmanEngine context)
        {
            if (context.GuessedLetters.Count > 0)
            {
                var lastLetter = context.GuessedLetters.Last();

                if (context.WordToGuess.Contains(lastLetter))
                {
                    return $"Nice! Letter found: {new string(context.CurrentGuess)}";
                }
                else
                {
                    return $"Oops! You have {context.MaxAttempts - context.WrongAttempts} tries left!";
                }
            }

            return "Let's start! Guess the word:";
        }
    }
}
