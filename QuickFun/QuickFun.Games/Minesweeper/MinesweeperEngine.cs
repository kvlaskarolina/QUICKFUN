using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using QuickFun.Domain.Enums;
using QuickFun.Games.Minesweeper.Strategies;
using QuickFun.Games.Base;

namespace QuickFun.Games.Minesweeper;

public class MinesweeperCell
{
    public int R { get; set; }
    public int C { get; set; }
    public bool IsMine { get; set; }
    public bool IsRevealed { get; set; }
    public bool IsFlagged { get; set; }
    public int AdjMines { get; set; }

    public bool ShouldShowNumber => IsRevealed && !IsMine && AdjMines > 0;
}

public class MinesweeperEngine : BaseGameEngine
{
    public event Action? OnStateChanged;
    public override GameType Type => GameType.Minesweeper;
    public override string Name => "Saper";
    public int Score { get; private set; } = 0;
    public MinesweeperCell[,] Board { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool IsWin { get; private set; }
    public string Message { get; private set; } = "GLHF!";


    private readonly IMinesweeperFloodingStrategy _strategy;
    private readonly int _rows = 12;
    private readonly int _cols = 12;
    private readonly int _minesCount = 20;
    private bool _isFirstMove = true;

    public event Action<int>? OnGameFinished;

    public MinesweeperEngine(IMinesweeperFloodingStrategy strategy)
    {
        _strategy = strategy;
        Reset();
    }
    public void Reset()
    {
        Board = new MinesweeperCell[_rows, _cols];
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _cols; c++)
            {
                Board[r, c] = new MinesweeperCell { R = r, C = c };
            }
        }

        _isFirstMove = true;
        IsGameOver = false;
        IsWin = false;
        Score = 0;
        Message = "GLHF!";
        OnStateChanged?.Invoke();
    }

    public void HandleClick(int r, int c)
    {
        if (IsGameOver || Board[r, c].IsFlagged || Board[r, c].IsRevealed) return;

        if (_isFirstMove)
        {
            GenerateMines(r, c);
            _isFirstMove = false;
        }

        if (Board[r, c].IsMine)
        {
            GameOver(false);
        }
        else
        {
            _strategy.flood(Board, r, c, _rows, _cols);
            CalculateScore();
            CheckWin();
        }

        OnStateChanged?.Invoke();
    }

    public void SwitchFlag(int r, int c)
    {
        if (IsGameOver || Board[r, c].IsRevealed) return;

        if (Board[r, c].IsFlagged)
        {
            Board[r, c].IsFlagged = false;
        }
        else
        {
            Board[r, c].IsFlagged = true;
        }

        OnStateChanged?.Invoke();
    }

    private void GenerateMines(int sr, int sc)
    {
        var random = new Random();
        int mines = 0;

        while (mines < _minesCount)
        {
            int r = random.Next(_rows);
            int c = random.Next(_cols);

            bool isInsideSafeZone = Math.Abs(r - sr) <= 1 && Math.Abs(c - sc) <= 1;

            if (isInsideSafeZone || Board[r, c].IsMine) continue;

            Board[r, c].IsMine = true;
            mines++;
        }

        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _cols; c++)
            {
                if (Board[r, c].IsMine) continue;

                int count = 0;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        int ni = r + i;
                        int nj = c + j;

                        if (ni >= 0 && ni < _rows && nj >= 0 && nj < _cols)
                        {
                            if (Board[ni, nj].IsMine) count++;
                        }
                    }
                }
                Board[r, c].AdjMines = count;
            }
        }
    }

    private void CheckWin()
    {
        int unrevealed = 0;

        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _cols; c++)
            {
                if (!Board[r, c].IsRevealed)
                {
                    unrevealed++;
                }
            }
        }

        if (unrevealed == _minesCount)
        {
            GameOver(true);
        }
    }

    private async void GameOver(bool outcome)
    {
        IsGameOver = true;
        IsWin = outcome;
        if (IsWin == true)
        {
            Message = "Mega big win!!!";
        }
        else
        {
            Message = "take the L";
        }
        
        OnGameFinished?.Invoke(Score);

        if (outcome)
        {
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    if (Board[r, c].IsMine && !Board[r, c].IsRevealed)
                    {
                        Board[r, c].IsRevealed = true;
                        OnStateChanged?.Invoke();
                        await Task.Delay(50);
                    }
                }
            }
        }
        else
        {
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    if (Board[r, c].IsMine) Board[r, c].IsRevealed = true;
                }
            }
            OnStateChanged?.Invoke();
        }
    }

    private void CalculateScore()
    {
        int total = 0;
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _cols; c++)
            {
                if (Board[r, c].IsRevealed && !Board[r, c].IsMine)
                {
                    int cellValue = Board[r, c].AdjMines switch
                    {
                        0 => 0,
                        1 or 2 => 2,
                        3 or 4 => 4,
                        _ => 10
                    };
                    total += cellValue;
                }
                
            }
        }

        Score = total;
    }

}
