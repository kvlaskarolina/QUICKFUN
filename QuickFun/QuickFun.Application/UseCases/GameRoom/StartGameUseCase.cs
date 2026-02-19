using System;
using System.Threading.Tasks;
using QuickFun.Domain.Entities;
using QuickFun.Infrastructure;

namespace QuickFun.Application.UseCases.GameRooms
{
    public class StartGameUseCase
    {
        private readonly IGameRoomRepository _roomRepository;

        public StartGameUseCase(IGameRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<bool> ExecuteAsync(Guid roomId)
        {
            return await _roomRepository.UpdateRoomStatusAsync(roomId, GameRoomStatus.InProgress);
        }
    }
}