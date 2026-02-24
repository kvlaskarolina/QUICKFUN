namespace QuickFun.Games.Memory.Strategies;

public interface IMemoryDifficultyStrategy
{
    string Name { get; }
    int Rows { get; }
    int Columns { get; }
    int DelayMs { get; }
    int ScorePerRound { get; }
}

public class EasyStrategyMemory : IMemoryDifficultyStrategy
{
    public string Name => "Łatwy";
    public int Rows => 4;
    public int Columns => 4;
    public int DelayMs => 3000;
    public int ScorePerRound => 5;
}

public class MediumStrategyMemory : IMemoryDifficultyStrategy
{
    public string Name => "Średni";
    public int Rows => 4;
    public int Columns => 6;
    public int DelayMs => 6000;
    public int ScorePerRound => 7;
}

public class HardStrategyMemory : IMemoryDifficultyStrategy
{
    public string Name => "Trudny";
    public int Rows => 6;
    public int Columns => 6;
    public int DelayMs => 10000;
    public int ScorePerRound => 9;
}



