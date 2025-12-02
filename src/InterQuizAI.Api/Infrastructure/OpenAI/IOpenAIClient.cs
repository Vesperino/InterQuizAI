namespace InterQuizAI.Api.Infrastructure.OpenAI;

public interface IOpenAIClient
{
    Task<QuizGenerationResult> GenerateQuizAsync(QuizGenerationRequest request, CancellationToken ct = default);
}

public record QuizGenerationRequest(
    string Language,
    string LanguageDisplayName,
    string Category,
    string CategoryDescription,
    string DifficultyLevel,
    string DifficultyDescription,
    string? Hint,
    int QuestionCount = 20
);

public record QuizGenerationResult(
    bool Success,
    List<GeneratedQuestion> Questions,
    string? ErrorMessage
);

public record GeneratedQuestion(
    string QuestionText,
    List<GeneratedAnswer> Answers,
    string? Explanation,
    string? SourceUrl,
    string? SourceTitle
);

public record GeneratedAnswer(string Text, bool IsCorrect);
