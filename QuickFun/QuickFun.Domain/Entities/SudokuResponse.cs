namespace QuickFun.Domain.Entities.Sudoku;

public class SudokuResponse
{
    public int[][] Board { get; set; } = default!;
    public string Difficulty { get; set; } = string.Empty;
    public int[][] Solution { get; set; } = default!;
}