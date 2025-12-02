using InterQuizAI.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InterQuizAI.Api.Features.History;

public interface IHistoryService
{
    Task<List<HistoryItemDto>> GetHistoryAsync(CancellationToken ct = default);
    Task<HistoryItemDto?> GetHistoryItemAsync(string sessionGuid, CancellationToken ct = default);
    Task<bool> DeleteHistoryItemAsync(string sessionGuid, CancellationToken ct = default);
    Task<HistoryStatsDto> GetStatsAsync(CancellationToken ct = default);
}

public class HistoryService : IHistoryService
{
    private readonly InterQuizDbContext _dbContext;

    public HistoryService(InterQuizDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<HistoryItemDto>> GetHistoryAsync(CancellationToken ct = default)
    {
        var sessions = await _dbContext.QuizSessions
            .Include(s => s.Language)
            .Include(s => s.Category)
            .Include(s => s.DifficultyLevel)
            .Include(s => s.Results)
            .OrderByDescending(s => s.StartedAt)
            .ToListAsync(ct);

        return sessions.Select(s => new HistoryItemDto(
            s.SessionGuid,
            s.Language.DisplayName,
            s.Category.DisplayName,
            s.DifficultyLevel.DisplayName,
            s.TotalQuestions,
            s.Results.Count(r => r.IsCorrect),
            s.TotalQuestions > 0 ? (double)s.Results.Count(r => r.IsCorrect) / s.TotalQuestions * 100 : 0,
            s.IsOfflineGenerated,
            s.StartedAt,
            s.CompletedAt,
            s.LanguageId,
            s.CategoryId,
            s.DifficultyLevelId,
            s.Hint
        )).ToList();
    }

    public async Task<HistoryItemDto?> GetHistoryItemAsync(string sessionGuid, CancellationToken ct = default)
    {
        var session = await _dbContext.QuizSessions
            .Include(s => s.Language)
            .Include(s => s.Category)
            .Include(s => s.DifficultyLevel)
            .Include(s => s.Results)
            .FirstOrDefaultAsync(s => s.SessionGuid == sessionGuid, ct);

        if (session == null)
            return null;

        return new HistoryItemDto(
            session.SessionGuid,
            session.Language.DisplayName,
            session.Category.DisplayName,
            session.DifficultyLevel.DisplayName,
            session.TotalQuestions,
            session.Results.Count(r => r.IsCorrect),
            session.TotalQuestions > 0 ? (double)session.Results.Count(r => r.IsCorrect) / session.TotalQuestions * 100 : 0,
            session.IsOfflineGenerated,
            session.StartedAt,
            session.CompletedAt,
            session.LanguageId,
            session.CategoryId,
            session.DifficultyLevelId,
            session.Hint
        );
    }

    public async Task<bool> DeleteHistoryItemAsync(string sessionGuid, CancellationToken ct = default)
    {
        var session = await _dbContext.QuizSessions
            .FirstOrDefaultAsync(s => s.SessionGuid == sessionGuid, ct);

        if (session == null)
            return false;

        _dbContext.QuizSessions.Remove(session);
        await _dbContext.SaveChangesAsync(ct);
        return true;
    }

    public async Task<HistoryStatsDto> GetStatsAsync(CancellationToken ct = default)
    {
        var sessions = await _dbContext.QuizSessions
            .Include(s => s.Language)
            .Include(s => s.DifficultyLevel)
            .Include(s => s.Results)
            .Where(s => s.IsCompleted)
            .ToListAsync(ct);

        var totalQuizzes = sessions.Count;
        var completedQuizzes = sessions.Count(s => s.IsCompleted);
        var totalQuestions = sessions.Sum(s => s.TotalQuestions);
        var totalCorrect = sessions.Sum(s => s.Results.Count(r => r.IsCorrect));

        var averageScore = totalQuestions > 0 ? (double)totalCorrect / totalQuestions * 100 : 0;

        var quizzesByLanguage = sessions
            .GroupBy(s => s.Language.DisplayName)
            .ToDictionary(g => g.Key, g => g.Count());

        var avgScoreByDifficulty = sessions
            .GroupBy(s => s.DifficultyLevel.DisplayName)
            .ToDictionary(
                g => g.Key,
                g =>
                {
                    var total = g.Sum(s => s.TotalQuestions);
                    var correct = g.Sum(s => s.Results.Count(r => r.IsCorrect));
                    return total > 0 ? (double)correct / total * 100 : 0;
                }
            );

        return new HistoryStatsDto(
            totalQuizzes,
            completedQuizzes,
            averageScore,
            totalQuestions,
            totalCorrect,
            quizzesByLanguage,
            avgScoreByDifficulty
        );
    }
}
