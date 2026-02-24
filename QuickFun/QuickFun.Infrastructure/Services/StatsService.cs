//opis data flow w QuickFun.Infrastructure.Services.LocalStorageGameSessionService.cs

using Microsoft.EntityFrameworkCore;
using QuickFun.Domain.Entities;
using QuickFun.Domain.Enums;
using QuickFun.Infrastructure.Data;

namespace QuickFun.Infrastructure.Services;
public class StatsService
{
    private readonly ApplicationDbContext _context;

    //dependency injection w konstruktorze,  przekazana w konstruktorze baza danych nie zmieni sie dzieki readonly
    public StatsService (ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task UpdateStatsAsync(string userId, int score, GameType gameType)
    {
        var stat = await _context.UserGameStats.FirstOrDefaultAsync(s => s.UserId == userId && s.GameType == gameType);

        if (stat == null)
        {
            stat = new UserGameStat
            {
                UserId = userId,
                GameType = gameType,
                BestScore = score,
                TotalScore = score,
                GamesPlayed = 1
            };
            _context.UserGameStats.Add(stat);
        }
        else
        {
            if (score > stat.BestScore)
            {
                stat.BestScore = score;
            }
            stat.TotalScore += score;
            stat.GamesPlayed++;
        }
        await _context.SaveChangesAsync();
    }
}