using InterQuizAI.Api.Features.Quiz.Dtos;

namespace InterQuizAI.Api.Features.Quiz;

public static class QuizEndpoints
{
    public static RouteGroupBuilder MapQuizEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/generate", GenerateQuiz);
        group.MapPost("/generate-offline", GenerateOfflineQuiz);
        group.MapPost("/repeat/{sessionGuid}", RepeatQuiz);
        group.MapGet("/{sessionGuid}", GetSession);
        group.MapGet("/{sessionGuid}/questions/{questionNumber:int}", GetQuestion);
        group.MapPost("/{sessionGuid}/answer", SubmitAnswer);
        group.MapPost("/{sessionGuid}/complete", CompleteQuiz);
        group.MapGet("/{sessionGuid}/results", GetResults);
        group.MapGet("/stored-count", GetStoredQuestionsCount);

        return group;
    }

    private static async Task<IResult> GenerateQuiz(GenerateQuizRequest request, IQuizService service, CancellationToken ct)
    {
        var result = await service.GenerateQuizAsync(request, ct);
        return result != null
            ? Results.Ok(result)
            : Results.BadRequest(new { error = "Nie udało się wygenerować quizu. Sprawdź master key i konfigurację API." });
    }

    private const int MinOfflineQuestions = 10;

    private static async Task<IResult> GenerateOfflineQuiz(GenerateQuizRequest request, IQuizService service, CancellationToken ct)
    {
        var count = await service.GetStoredQuestionsCountAsync(request.LanguageId, request.CategoryId, request.DifficultyLevelId, ct);

        if (count < MinOfflineQuestions)
        {
            return Results.BadRequest(new { error = $"Za mało zapisanych pytań ({count}/{MinOfflineQuestions}). Wygeneruj najpierw quiz online." });
        }

        var result = await service.GenerateOfflineQuizAsync(request, ct);
        return result != null
            ? Results.Ok(result)
            : Results.BadRequest(new { error = "Nie udało się wygenerować quizu offline." });
    }

    private static async Task<IResult> GetSession(string sessionGuid, IQuizService service, CancellationToken ct)
    {
        var session = await service.GetSessionAsync(sessionGuid, ct);
        return session != null ? Results.Ok(session) : Results.NotFound();
    }

    private static async Task<IResult> GetQuestion(string sessionGuid, int questionNumber, IQuizService service, CancellationToken ct)
    {
        var question = await service.GetQuestionAsync(sessionGuid, questionNumber, ct);
        return question != null ? Results.Ok(question) : Results.NotFound();
    }

    private static async Task<IResult> SubmitAnswer(string sessionGuid, SubmitAnswerRequest request, IQuizService service, CancellationToken ct)
    {
        var result = await service.SubmitAnswerAsync(sessionGuid, request, ct);
        return result ? Results.Ok() : Results.BadRequest(new { error = "Nie udało się zapisać odpowiedzi" });
    }

    private static async Task<IResult> CompleteQuiz(string sessionGuid, IQuizService service, CancellationToken ct)
    {
        var results = await service.CompleteQuizAsync(sessionGuid, ct);
        return results != null ? Results.Ok(results) : Results.NotFound();
    }

    private static async Task<IResult> GetResults(string sessionGuid, IQuizService service, CancellationToken ct)
    {
        var results = await service.GetResultsAsync(sessionGuid, ct);
        return results != null ? Results.Ok(results) : Results.NotFound();
    }

    private static async Task<IResult> GetStoredQuestionsCount(
        int languageId,
        int categoryId,
        int difficultyLevelId,
        IQuizService service,
        CancellationToken ct)
    {
        var count = await service.GetStoredQuestionsCountAsync(languageId, categoryId, difficultyLevelId, ct);
        return Results.Ok(new { count, canGenerateOffline = count >= MinOfflineQuestions });
    }

    private static async Task<IResult> RepeatQuiz(string sessionGuid, IQuizService service, CancellationToken ct)
    {
        var result = await service.RepeatQuizAsync(sessionGuid, ct);
        return result != null
            ? Results.Ok(result)
            : Results.NotFound(new { error = "Nie znaleziono quizu do powtórzenia." });
    }
}
