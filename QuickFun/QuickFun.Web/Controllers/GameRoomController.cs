using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuickFun.Application.UseCases.GameRooms;
using QuickFun.Infrastructure;
using QuickFun.Domain.Entities;

namespace QuickFun.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameRoomsController : ControllerBase
    {
        private readonly IGameRoomRepository _roomRepository;

        public GameRoomsController(IGameRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        /// <summary>
        /// Utwórz nowy pokój gry
        /// POST: /api/gamerooms/create
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest request)
        {
            try
            {
                var useCase = new CreateGameRoomUseCase(_roomRepository);
                var room = await useCase.ExecuteAsync(
                    request.RoomName,
                    request.CreatorId,
                    request.GameType,
                    request.MaxPlayers
                );

                return Ok(new
                {
                    success = true,
                    roomId = room.Id,
                    room.Name,
                    room.Status,
                    room.GameType,
                    room.MaxPlayers
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Pobierz listę dostępnych pokoi
        /// GET: /api/gamerooms/available
        /// </summary>
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableRooms()
        {
            try
            {
                var useCase = new GetAvailableRoomsUseCase(_roomRepository);
                var rooms = await useCase.ExecuteAsync();

                return Ok(new
                {
                    success = true,
                    count = rooms.Count,
                    rooms = rooms.Select(r => new
                    {
                        r.Id,
                        r.Name,
                        r.GameType,
                        r.Status,
                        playersCount = r.Players.Count,
                        r.MaxPlayers,
                        players = r.Players.Select(p => new { p.PlayerName, p.Status })
                    })
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Dołącz do pokoju
        /// POST: /api/gamerooms/{roomId}/join
        /// </summary>
        [HttpPost("{roomId}/join")]
        public async Task<IActionResult> JoinRoom(Guid roomId, [FromBody] JoinRoomRequest request)
        {
            try
            {
                var useCase = new JoinGameRoomUseCase(_roomRepository);
                var success = await useCase.ExecuteAsync(roomId, request.UserId, request.PlayerName);

                if (!success)
                    return BadRequest(new
                    {
                        success = false,
                        error = "Nie można dołączyć do pokoju. Pokój jest pełny lub niedostępny."
                    });

                var room = await _roomRepository.GetByIdAsync(roomId);
                return Ok(new
                {
                    success = true,
                    message = $"✅ Dołączyłeś do pokoju '{room.Name}'",
                    playersCount = room.Players.Count,
                    maxPlayers = room.MaxPlayers
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Opuść pokój
        /// DELETE: /api/gamerooms/{roomId}/leave?userId={userId}
        /// </summary>
        [HttpDelete("{roomId}/leave")]
        public async Task<IActionResult> LeaveRoom(Guid roomId, [FromQuery] Guid userId)
        {
            try
            {
                var success = await _roomRepository.LeaveRoomAsync(roomId, userId);

                if (!success)
                    return BadRequest(new { success = false, error = "Nie można opuścić pokoju." });

                return Ok(new { success = true, message = "✅ Opuściłeś pokój" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Rozpocznij grę
        /// POST: /api/gamerooms/{roomId}/start
        /// </summary>
        [HttpPost("{roomId}/start")]
        public async Task<IActionResult> StartGame(Guid roomId)
        {
            try
            {
                var useCase = new StartGameUseCase(_roomRepository);
                var success = await useCase.ExecuteAsync(roomId);

                if (!success)
                    return BadRequest(new { success = false, error = "Nie można rozpocząć gry." });

                return Ok(new { success = true, message = "🎮 Gra rozpoczęta!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
    }

    public class CreateRoomRequest
    {
        public string RoomName { get; set; }
        public Guid CreatorId { get; set; }
        public string GameType { get; set; } // "TicTacToe"
        public int MaxPlayers { get; set; } = 2;
    }

    public class JoinRoomRequest
    {
        public Guid UserId { get; set; }
        public string PlayerName { get; set; }
    }
}