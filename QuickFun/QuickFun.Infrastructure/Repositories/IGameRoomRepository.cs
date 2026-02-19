using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuickFun.Domain.Entities;

namespace QuickFun.Infrastructure
{
    public interface IGameRoomRepository
    {
        Task<GameRoom> CreateAsync(GameRoom room);
        Task<GameRoom> GetByIdAsync(Guid id);
        Task<List<GameRoom>> GetAvailableRoomsAsync();
        Task<bool> JoinRoomAsync(Guid roomId, RoomPlayer player);
        Task<bool> LeaveRoomAsync(Guid roomId, Guid userId);
        Task<bool> UpdateRoomStatusAsync(Guid roomId, GameRoomStatus status);
        Task<bool> DeleteAsync(Guid id);
    }
}