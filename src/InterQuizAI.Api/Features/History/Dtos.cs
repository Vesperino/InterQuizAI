namespace InterQuizAI.Api.Features.History;

public record HistoryItemDto(
    string SessionGuid,
    string Language,
    string Category,
    string DifficultyLevel,
    int TotalQuestions,
    int CorrectAnswers,
    double ScorePercentage,
    bool IsOfflineGenerated,
    DateTime StartedAt,
    DateTime? CompletedAt,
    int LanguageId,
    int CategoryId,
    int DifficultyLevelId,
    string? Hint
);

public record HistoryStatsDto(
    int TotalQuizzes,
    int CompletedQuizzes,
    double AverageScore,
    int TotalQuestions,
    int TotalCorrectAnswers,
    Dictionary<string, int> QuizzesByLanguage,
    Dictionary<string, double> AverageScoreByDifficulty
);
