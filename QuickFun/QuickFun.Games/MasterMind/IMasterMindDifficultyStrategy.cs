namespace QuickFun.Games.MasterMind.Strategies;

public interface IMasterMindDifficultyStrategy
{
    string Name { get; }
    int CodeLength { get; }
    int MaxAttempts { get; }
    int Colors { get; }
    int ScorePerRound { get; }
}

public class EasyStrategyMasterMind : IMasterMindDifficultyStrategy
{
    public string Name => "Easy";
    public int CodeLength => 4;
    public int MaxAttempts => 10;
    public int Colors => 4;
    public int ScorePerRound => 5;
}

public class MediumStrategyMasterMind : IMasterMindDifficultyStrategy
{
    public string Name => "Medium";
    public int CodeLength => 5;
    public int MaxAttempts => 10;
    public int Colors => 5;
    public int ScorePerRound => 7;
}

public class HardStrategyMasterMind : IMasterMindDifficultyStrategy
{
    public string Name => "Hard";
    public int CodeLength => 6;
    public int Colors => 6;
    public int MaxAttempts => 10;
    public int ScorePerRound => 9;
}



