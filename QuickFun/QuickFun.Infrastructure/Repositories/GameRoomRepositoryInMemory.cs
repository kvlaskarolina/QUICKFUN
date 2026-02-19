using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuickFun.Domain.Entities;

namespace QuickFun.Infrastructure
{
    public class GameRoomRepositoryInMemory : IGameRoomRepository
    {
        private static Dictionary<Guid, GameRoom> _rooms = new Dictionary<Guid, GameRoom>();
        private static object _lockObject = new object();

        public Task<GameRoom> CreateAsync(GameRoom room)
        {
            lock (_lockObject)
            {
                _rooms[room.Id] = room;
                Console.WriteLine($"✅ Pokój '{room.Name}' został utworzony (ID: {room.Id})");
                return Task.FromResult(room);
            }
        }

        public Task<GameRoom> GetByIdAsync(Guid id)
        {
            lock (_lockObject)
            {
                _rooms.TryGetValue(id, out var room);
                return Task.FromResult(room);
            }
        }

        public Task<List<GameRoom>> GetAvailableRoomsAsync()
        {
            lock (_lockObject)
            {
                var available = _rooms.Values
                    .Where(r => r.Status == GameRoomStatus.Waiting && r.Players.Count < r.MaxPlayers)
                    .ToList();
                Console.WriteLine($"📋 Znaleziono {available.Count} dostępnych pokoi");
                return Task.FromResult(available);
            }
        }

        public Task<bool> JoinRoomAsync(Guid roomId, RoomPlayer player)
        {
            lock (_lockObject)
            {
                if (!_rooms.TryGetValue(roomId, out var room))
                {
                    Console.WriteLine($"❌ Pokój nie istnieje (ID: {roomId})");
                    return Task.FromResult(false);
                }

                if (room.Players.Count >= room.MaxPlayers)
                {
                    Console.WriteLine($"❌ Pokój '{room.Name}' jest pełny!");
                    return Task.FromResult(false);
                }

                room.Players.Add(player);
                Console.WriteLine($"✅ Gracz '{player.PlayerName}' dołączył do pokoju '{room.Name}'");
                Console.WriteLine($"   Graczy w pokoju: {room.Players.Count}/{room.MaxPlayers}");
                return Task.FromResult(true);
            }
        }

        public Task<bool> LeaveRoomAsync(Guid roomId, Guid userId)
        {
            lock (_lockObject)
            {
                if (!_rooms.TryGetValue(roomId, out var room))
                {
                    Console.WriteLine($"❌ Pokój nie istnieje (ID: {roomId})");
                    return Task.FromResult(false);
                }

                var player = room.Players.FirstOrDefault(p => p.UserId == userId);
                if (player != null)
                {
                    room.Players.Remove(player);
                    Console.WriteLine($"✅ Gracz '{player.PlayerName}' opuścił pokój '{room.Name}'");
                    Console.WriteLine($"   Graczy w pokoju: {room.Players.Count}/{room.MaxPlayers}");

                    if (room.Players.Count == 0)
                    {
                        _rooms.Remove(roomId);
                        Console.WriteLine($"🗑️  Pokój '{room.Name}' został usunięty (brak graczy)");
                    }
                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
        }

        public Task<bool> UpdateRoomStatusAsync(Guid roomId, GameRoomStatus status)
        {
            lock (_lockObject)
            {
                if (!_rooms.TryGetValue(roomId, out var room))
                {
                    Console.WriteLine($"❌ Pokój nie istnieje (ID: {roomId})");
                    return Task.FromResult(false);
                }

                room.Status = status;
                if (status == GameRoomStatus.InProgress)
                    room.StartedAt = DateTime.UtcNow;

                Console.WriteLine($"✅ Status pokoju '{room.Name}' zmieniony na: {status}");
                return Task.FromResult(true);
            }
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            lock (_lockObject)
            {
                if (_rooms.Remove(id))
                {
                    Console.WriteLine($"✅ Pokój został usunięty (ID: {id})");
                    return Task.FromResult(true);
                }
                Console.WriteLine($"❌ Pokój nie istnieje (ID: {id})");
                return Task.FromResult(false);
            }
        }
    }
}