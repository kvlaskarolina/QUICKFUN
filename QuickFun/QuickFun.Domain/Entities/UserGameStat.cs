using Microsoft.AspNetCore.Identity;
using QuickFun.Domain.Enums;

namespace QuickFun.Domain.Entities;

public class UserGameStat
{
    public int Id { get; set; } //primary key
    public string UserId { get; set; } = ""; //foregin key
    public IdentityUser? User { get; set; }
    public GameType GameType { get; set; }
    public int BestScore { get; set; }
    public int TotalScore { get; set; }
    public int GamesPlayed { get; set; }

}