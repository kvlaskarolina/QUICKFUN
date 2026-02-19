using QuickFun.Domain.Entities;
using QuickFun.Domain.Enums;


public interface IGameSessionService
{
    Task SavePlayerNameAsync(string name);
    Task<string> GetPlayerNameAsync();
    Task AddGameResultAsync(GameResult result);
    Task<List<GameResult>> GetSessionHistoryAsync();
}