using System;
using System.Threading.Tasks;
using QuickFun.Domain.Entities;

namespace QuickFun.Application.Interfaces
{
    public interface IGameSessionService
    {
        Task<bool> StartGameAsync(Guid roomId);
        Task<bool> EndGameAsync(Guid roomId);
        Task<string> GetGameStatusAsync(Guid roomId);
        Task AddGameResultAsync(GameResult result);
    }
}