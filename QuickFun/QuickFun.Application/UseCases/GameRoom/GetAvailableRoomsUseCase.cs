using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuickFun.Domain.Entities;
using QuickFun.Infrastructure;

namespace QuickFun.Application.UseCases.GameRooms
{
    public class GetAvailableRoomsUseCase
    {
        private readonly IGameRoomRepository _roomRepository;

        public GetAvailableRoomsUseCase(IGameRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<List<GameRoom>> ExecuteAsync()
        {
            return await _roomRepository.GetAvailableRoomsAsync();
        }
    }
}