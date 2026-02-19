using System;
using System.Threading.Tasks;
using QuickFun.Domain.Entities;
using QuickFun.Infrastructure;

namespace QuickFun.Application.UseCases.GameRooms
{
    public class CreateGameRoomUseCase
    {
        private readonly Infrastructure.IGameRoomRepository _roomRepository;

        public CreateGameRoomUseCase(IGameRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<GameRoom> ExecuteAsync(string roomName, Guid creatorId, string gameType, int maxPlayers = 2)
        {
            var room = new GameRoom
            {
                Id = Guid.NewGuid(),
                Name = roomName,
                CreatorId = creatorId,
                GameType = gameType,
                Status = GameRoomStatus.Waiting,
                MaxPlayers = maxPlayers,
                CreatedAt = DateTime.UtcNow
            };

            return await _roomRepository.CreateAsync(room);
        }
    }
}