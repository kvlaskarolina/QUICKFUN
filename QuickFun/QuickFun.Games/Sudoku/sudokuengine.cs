using QuickFun.Domain.Entities.Sudoku;
using QuickFun.Domain.Enums;
using System.Net.Http.Json;
using QuickFun.Games.Base;
namespace QuickFun.Games.Engines.Sudoku;


public class SudokuEngine : BaseGameEngine
{
    private readonly HttpClient _httpClient;
    public override string Name => "Sudoku";
    public string Result { get; private set; }
    public override GameType Type => GameType.Sudoku;
    public string Difficulty { get; private set; }
    public int Score { get; private set; }
    public int[][]? Board { get; private set; }
    public int[][]? Solution { get; private set; }
    public bool[][]? IsOriginal { get; private set; }
    public bool IsLoading { get; private set; }

    public SudokuEngine(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Start(Func<Task> onStateChanged) => await LoadBoard("medium", onStateChanged);

    public async Task LoadBoard(string difficulty, Func<Task> onStateChanged)
    {
        IsLoading = true;
        await onStateChanged();

        try
        {
            var response = await _httpClient.GetFromJsonAsync<SudokuResponse>($"api/sudoku/generate?difficulty={difficulty}");
            if (response?.Board != null)
            {
                Board = response.Board;
                Solution = response.Solution;
                Result = string.Empty;
                Difficulty = response.Difficulty;
                Score = 0;
                IsOriginal = new bool[9][];
                for (int r = 0; r < 9; r++)
                {
                    IsOriginal[r] = new bool[9];
                    for (int c = 0; c < 9; c++)
                        IsOriginal[r][c] = Board[r][c] != 0;
                }
            }
        }
        catch (Exception ex) { Console.WriteLine($"Błąd: {ex.Message}"); }
        finally
        {
            IsLoading = false;
            await onStateChanged();
        }
    }

    public void UpdateCell(int row, int col, int value)
    {
        if (Board != null && IsOriginal != null && row < 9 && col < 9)
        {
            if (!IsOriginal[row][col])
            {
                Board[row][col] = value;
            }
        }
    }

    public void CheckWin()
    {
        if (Board == null || Solution == null) Result = "take the L";
        for (int r = 0; r < 9; r++)
            for (int c = 0; c < 9; c++)
                if (Board[r][c] != Solution[r][c])
                {
                    Result = "take the L";
                    Score = 0;
                    return;
                }
        Result = "Kudos goon job";
        Score = Difficulty.ToLower() switch
        {
            "easy" => 100,
            "medium" => 200,
            "hard" => 300,
            _ => 30
        };

    }

    public void Reset() => Board = null;
}