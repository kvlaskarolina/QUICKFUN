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

    public async Task<List<LeaderboardDto>> GetTopScoresAsync(GameType gameType)
    {
        return await _context.UserGameStats.Where(s => s.GameType == gameType)
            .OrderByDescending(s => s.BestScore)
            .Take(5)
            .Select(s => new LeaderboardDto(s.User!.UserName ?? "Unknown", s.BestScore, s.TotalScore))
            .ToListAsync();
    }

    public async Task<List<UserStatDto>> GetUserStatsAsync(string userId)
    {
        return await _context.UserGameStats.Where(s => s.UserId == userId)
            .Select(s => new UserStatDto(s.GameType.ToString(), s.BestScore, s.TotalScore, s.GamesPlayed))
        .ToListAsync();
    }
}