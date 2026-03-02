using QuickFun.Domain.Enums;
using QuickFun.Games.Engines;
using QuickFun.Games.Memory;
using QuickFun.Games.Hangman;
using Microsoft.Extensions.Http;
using System.Net.Http;
using QuickFun.Games.Engines.Sudoku;
using QuickFun.Games.Minesweeper;
using QuickFun.Games.Engines.TicTacToe;
using QuickFun.Games.Engines.TicTacToe.AI;
using QuickFun.Games.TicTacToe.Strategies;
using QuickFun.Games.Engines.MasterMind;

namespace QuickFun.Games
{
    public interface IGameFactory
    {
        IGameEngine CreateGame(GameType type);
    }

    public class GameFactory : IGameFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GameFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IGameEngine CreateGame(GameType type)
        {
            return type switch
            {

                GameType.TicTacToeWithAI => new TicTacToeEngineWithAI(),
                GameType.TicTacToe => new TicTacToeEngine(),
                GameType.Memory => new MemoryEngine(),
                GameType.Minesweeper => new MinesweeperEngine(new QuickFun.Games.Minesweeper.Strategies.DfsFloodingStrategy()),
                GameType.Hangman => HangmanFactory.CreateGame(),
                GameType.Sudoku => new SudokuEngine(_httpClientFactory.CreateClient("SudokuApi")),
                GameType.MasterMind => new MasterMindEngine(),
                _ => throw new ArgumentException("Nieznana gra")
            };
        }
    }
}