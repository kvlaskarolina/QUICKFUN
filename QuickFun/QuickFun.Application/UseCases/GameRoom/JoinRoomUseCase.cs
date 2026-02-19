using System;
using System.Threading.Tasks;
using QuickFun.Domain.Entities;
using QuickFun.Infrastructure;

namespace QuickFun.Application.UseCases.GameRooms
{
    public class JoinGameRoomUseCase
    {
        private readonly IGameRoomRepository _roomRepository;

        public JoinGameRoomUseCase(IGameRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<bool> ExecuteAsync(Guid roomId, Guid userId, string playerName)
        {
            // ✅ Pobierz pokój najpierw
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null)
                return false;

            var player = new RoomPlayer
            {
                Id = Guid.NewGuid(),
                RoomId = roomId,
                UserId = userId,
                PlayerName = playerName,
                Status = PlayerStatus.Waiting,
                JoinedAt = DateTime.UtcNow,
                GameRoom = room
            };

            return await _roomRepository.JoinRoomAsync(roomId, player);
        }
    }
}