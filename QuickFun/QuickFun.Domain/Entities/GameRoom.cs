using System;
using System.Collections.Generic;

namespace QuickFun.Domain.Entities
{
    public class GameRoom
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid CreatorId { get; set; }
        public required string GameType { get; set; }
        public GameRoomStatus Status { get; set; }
        public int MaxPlayers { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartedAt { get; set; }

        public virtual ICollection<RoomPlayer> Players { get; set; } = new List<RoomPlayer>();
        public Guid? CurrentGameId { get; set; }
    }

    public class RoomPlayer
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public required string PlayerName { get; set; }
        public PlayerStatus Status { get; set; }
        public DateTime JoinedAt { get; set; }

        public required virtual GameRoom GameRoom { get; set; }
    }

    public enum GameRoomStatus
    {
        Waiting,
        InProgress,
        Finished,
        Canceled
    }

    public enum PlayerStatus
    {
        Waiting,
        Playing,
        Finished,
        Disconnected
    }
}