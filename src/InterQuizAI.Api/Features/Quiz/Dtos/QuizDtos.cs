namespace InterQuizAI.Api.Features.Quiz.Dtos;

public record GenerateQuizRequest(
    int LanguageId,
    int CategoryId,
    int DifficultyLevelId,
    string? Hint,
    string MasterKey,
    string QuizLanguage = "pl"  // "pl" for Polish, "en" for English
);

public record QuizSessionDto(
    string SessionGuid,
    string Language,
    string Category,
    string DifficultyLevel,
    string? Hint,
    int TotalQuestions,
    bool IsCompleted,
    bool IsOfflineGenerated,
    DateTime StartedAt
);

public record QuestionDto(
    int QuestionNumber,
    string QuestionId,
    string QuestionText,
    List<AnswerOptionDto> Answers
);

public record AnswerOptionDto(
    int Id,
    string Text,
    int Order
);

public record SubmitAnswerRequest(
    string QuestionId,
    int? SelectedAnswerId
);

public record QuizResultsDto(
    string SessionGuid,
    string Language,
    string Category,
    string DifficultyLevel,
    int TotalQuestions,
    int CorrectAnswers,
    double ScorePercentage,
    DateTime StartedAt,
    DateTime? CompletedAt,
    List<QuestionResultDto> Questions
);

public record QuestionResultDto(
    int QuestionNumber,
    string QuestionText,
    List<AnswerResultDto> Answers,
    int? SelectedAnswerId,
    bool IsCorrect,
    string? Explanation,
    string? SourceUrl,
    string? SourceTitle
);

public record AnswerResultDto(
    int Id,
    string Text,
    bool IsCorrect,
    bool IsSelected
);
