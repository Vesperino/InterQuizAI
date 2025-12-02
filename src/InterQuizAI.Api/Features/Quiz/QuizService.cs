using InterQuizAI.Api.Domain.Entities;
using InterQuizAI.Api.Features.Configuration;
using InterQuizAI.Api.Features.Quiz.Dtos;
using InterQuizAI.Api.Infrastructure.OpenAI;
using InterQuizAI.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InterQuizAI.Api.Features.Quiz;

public interface IQuizService
{
    Task<QuizSessionDto?> GenerateQuizAsync(GenerateQuizRequest request, CancellationToken ct = default);
    Task<QuizSessionDto?> GenerateOfflineQuizAsync(GenerateQuizRequest request, CancellationToken ct = default);
    Task<QuizSessionDto?> RepeatQuizAsync(string originalSessionGuid, CancellationToken ct = default);
    Task<QuizSessionDto?> GetSessionAsync(string sessionGuid, CancellationToken ct = default);
    Task<QuestionDto?> GetQuestionAsync(string sessionGuid, int questionNumber, CancellationToken ct = default);
    Task<bool> SubmitAnswerAsync(string sessionGuid, SubmitAnswerRequest answer, CancellationToken ct = default);
    Task<QuizResultsDto?> CompleteQuizAsync(string sessionGuid, CancellationToken ct = default);
    Task<QuizResultsDto?> GetResultsAsync(string sessionGuid, CancellationToken ct = default);
    Task<int> GetStoredQuestionsCountAsync(int languageId, int categoryId, int difficultyLevelId, CancellationToken ct = default);
}

public class QuizService : IQuizService
{
    private readonly InterQuizDbContext _dbContext;
    private readonly IConfigurationService _configService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<QuizService> _logger;

    public QuizService(
        InterQuizDbContext dbContext,
        IConfigurationService configService,
        IHttpClientFactory httpClientFactory,
        ILogger<QuizService> logger)
    {
        _dbContext = dbContext;
        _configService = configService;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<QuizSessionDto?> GenerateQuizAsync(GenerateQuizRequest request, CancellationToken ct = default)
    {
        // Get API key and model
        var apiKey = await _configService.GetDecryptedApiKeyAsync(request.MasterKey, ct);
        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogWarning("Invalid master key or no API key configured");
            return null;
        }

        var modelName = await _configService.GetModelAsync(ct) ?? "gpt-4o";

        // Get language, category, difficulty details
        var language = await _dbContext.ProgrammingLanguages.FindAsync(new object[] { request.LanguageId }, ct);
        var category = await _dbContext.Categories.FindAsync(new object[] { request.CategoryId }, ct);
        var difficulty = await _dbContext.DifficultyLevels.FindAsync(new object[] { request.DifficultyLevelId }, ct);

        if (language == null || category == null || difficulty == null)
        {
            _logger.LogWarning("Invalid language, category, or difficulty level");
            return null;
        }

        // Create OpenAI client and generate quiz
        var httpClient = _httpClientFactory.CreateClient();
        var openAiClient = new OpenAIClient(httpClient, _logger as ILogger<OpenAIClient> ?? LoggerFactory.Create(b => b.AddConsole()).CreateLogger<OpenAIClient>(), apiKey, modelName);

        var generationRequest = new QuizGenerationRequest(
            language.Name,
            language.DisplayName,
            category.DisplayName,
            category.Description ?? "",
            difficulty.DisplayName,
            difficulty.Description ?? "",
            request.Hint,
            request.QuizLanguage ?? "pl",
            20
        );

        var result = await openAiClient.GenerateQuizAsync(generationRequest, ct);

        if (!result.Success || result.Questions.Count == 0)
        {
            _logger.LogError("Quiz generation failed: {Error}", result.ErrorMessage);
            return null;
        }

        // Create quiz session
        var session = new QuizSession
        {
            LanguageId = request.LanguageId,
            CategoryId = request.CategoryId,
            DifficultyLevelId = request.DifficultyLevelId,
            Hint = request.Hint,
            TotalQuestions = result.Questions.Count,
            IsOfflineGenerated = false
        };

        _dbContext.QuizSessions.Add(session);
        await _dbContext.SaveChangesAsync(ct);

        // Save questions to database
        var order = 1;
        foreach (var genQuestion in result.Questions)
        {
            var question = new Question
            {
                LanguageId = request.LanguageId,
                CategoryId = request.CategoryId,
                DifficultyLevelId = request.DifficultyLevelId,
                QuestionText = genQuestion.QuestionText,
                Explanation = genQuestion.Explanation,
                SourceUrl = genQuestion.SourceUrl,
                SourceTitle = genQuestion.SourceTitle,
                Hint = request.Hint,
                IsVerified = true
            };

            var answerOrder = 1;
            foreach (var genAnswer in genQuestion.Answers)
            {
                question.Answers.Add(new Answer
                {
                    AnswerText = genAnswer.Text,
                    IsCorrect = genAnswer.IsCorrect,
                    SortOrder = answerOrder++
                });
            }

            _dbContext.Questions.Add(question);
            await _dbContext.SaveChangesAsync(ct);

            session.SessionQuestions.Add(new QuizSessionQuestion
            {
                QuestionId = question.Id,
                QuestionOrder = order++
            });
        }

        await _dbContext.SaveChangesAsync(ct);

        return new QuizSessionDto(
            session.SessionGuid,
            language.DisplayName,
            category.DisplayName,
            difficulty.DisplayName,
            request.Hint,
            session.TotalQuestions,
            false,
            false,
            session.StartedAt
        );
    }

    private const int MinOfflineQuestions = 10;
    private const int MaxOfflineQuestions = 20;

    public async Task<QuizSessionDto?> GenerateOfflineQuizAsync(GenerateQuizRequest request, CancellationToken ct = default)
    {
        // Get random questions from stored database (up to 20)
        var questions = await _dbContext.Questions
            .Include(q => q.Answers)
            .Where(q => q.LanguageId == request.LanguageId
                     && q.CategoryId == request.CategoryId
                     && q.DifficultyLevelId == request.DifficultyLevelId)
            .OrderBy(q => Guid.NewGuid())
            .Take(MaxOfflineQuestions)
            .ToListAsync(ct);

        if (questions.Count < MinOfflineQuestions)
        {
            _logger.LogWarning("Not enough stored questions. Found {Count}, need {Min}", questions.Count, MinOfflineQuestions);
            return null;
        }

        var language = await _dbContext.ProgrammingLanguages.FindAsync(new object[] { request.LanguageId }, ct);
        var category = await _dbContext.Categories.FindAsync(new object[] { request.CategoryId }, ct);
        var difficulty = await _dbContext.DifficultyLevels.FindAsync(new object[] { request.DifficultyLevelId }, ct);

        if (language == null || category == null || difficulty == null)
            return null;

        var questionCount = questions.Count;

        var session = new QuizSession
        {
            LanguageId = request.LanguageId,
            CategoryId = request.CategoryId,
            DifficultyLevelId = request.DifficultyLevelId,
            Hint = request.Hint,
            TotalQuestions = questionCount,
            IsOfflineGenerated = true
        };

        _dbContext.QuizSessions.Add(session);

        var order = 1;
        foreach (var question in questions)
        {
            session.SessionQuestions.Add(new QuizSessionQuestion
            {
                QuestionId = question.Id,
                QuestionOrder = order++
            });
        }

        await _dbContext.SaveChangesAsync(ct);

        return new QuizSessionDto(
            session.SessionGuid,
            language.DisplayName,
            category.DisplayName,
            difficulty.DisplayName,
            request.Hint,
            questionCount,
            false,
            true,
            session.StartedAt
        );
    }

    public async Task<QuizSessionDto?> GetSessionAsync(string sessionGuid, CancellationToken ct = default)
    {
        var session = await _dbContext.QuizSessions
            .Include(s => s.Language)
            .Include(s => s.Category)
            .Include(s => s.DifficultyLevel)
            .FirstOrDefaultAsync(s => s.SessionGuid == sessionGuid, ct);

        if (session == null)
            return null;

        return new QuizSessionDto(
            session.SessionGuid,
            session.Language.DisplayName,
            session.Category.DisplayName,
            session.DifficultyLevel.DisplayName,
            session.Hint,
            session.TotalQuestions,
            session.IsCompleted,
            session.IsOfflineGenerated,
            session.StartedAt
        );
    }

    public async Task<QuestionDto?> GetQuestionAsync(string sessionGuid, int questionNumber, CancellationToken ct = default)
    {
        var sessionQuestion = await _dbContext.QuizSessionQuestions
            .Include(sq => sq.Session)
            .Include(sq => sq.Question)
                .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(sq => sq.Session.SessionGuid == sessionGuid && sq.QuestionOrder == questionNumber, ct);

        if (sessionQuestion == null)
            return null;

        var question = sessionQuestion.Question;

        return new QuestionDto(
            questionNumber,
            question.ExternalId,
            question.QuestionText,
            question.Answers.OrderBy(a => a.SortOrder).Select(a => new AnswerOptionDto(a.Id, a.AnswerText, a.SortOrder)).ToList()
        );
    }

    public async Task<bool> SubmitAnswerAsync(string sessionGuid, SubmitAnswerRequest answer, CancellationToken ct = default)
    {
        var session = await _dbContext.QuizSessions
            .Include(s => s.SessionQuestions)
                .ThenInclude(sq => sq.Question)
            .FirstOrDefaultAsync(s => s.SessionGuid == sessionGuid, ct);

        if (session == null || session.IsCompleted)
            return false;

        var sessionQuestion = session.SessionQuestions
            .FirstOrDefault(sq => sq.Question.ExternalId == answer.QuestionId);

        if (sessionQuestion == null)
            return false;

        var correctAnswer = await _dbContext.Answers
            .FirstOrDefaultAsync(a => a.QuestionId == sessionQuestion.QuestionId && a.IsCorrect, ct);

        var isCorrect = answer.SelectedAnswerId.HasValue && correctAnswer?.Id == answer.SelectedAnswerId;

        // Check if result already exists
        var existingResult = await _dbContext.QuizResults
            .FirstOrDefaultAsync(r => r.SessionId == session.Id && r.QuestionId == sessionQuestion.QuestionId, ct);

        if (existingResult != null)
        {
            existingResult.SelectedAnswerId = answer.SelectedAnswerId;
            existingResult.IsCorrect = isCorrect;
            existingResult.AnsweredAt = DateTime.UtcNow;
        }
        else
        {
            _dbContext.QuizResults.Add(new QuizResult
            {
                SessionId = session.Id,
                QuestionId = sessionQuestion.QuestionId,
                SelectedAnswerId = answer.SelectedAnswerId,
                IsCorrect = isCorrect
            });
        }

        await _dbContext.SaveChangesAsync(ct);
        return true;
    }

    public async Task<QuizResultsDto?> CompleteQuizAsync(string sessionGuid, CancellationToken ct = default)
    {
        var session = await _dbContext.QuizSessions
            .FirstOrDefaultAsync(s => s.SessionGuid == sessionGuid, ct);

        if (session == null)
            return null;

        session.IsCompleted = true;
        session.CompletedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(ct);

        return await GetResultsAsync(sessionGuid, ct);
    }

    public async Task<QuizResultsDto?> GetResultsAsync(string sessionGuid, CancellationToken ct = default)
    {
        var session = await _dbContext.QuizSessions
            .Include(s => s.Language)
            .Include(s => s.Category)
            .Include(s => s.DifficultyLevel)
            .Include(s => s.SessionQuestions)
                .ThenInclude(sq => sq.Question)
                    .ThenInclude(q => q.Answers)
            .Include(s => s.Results)
            .FirstOrDefaultAsync(s => s.SessionGuid == sessionGuid, ct);

        if (session == null)
            return null;

        var questionResults = new List<QuestionResultDto>();
        var correctCount = 0;

        foreach (var sq in session.SessionQuestions.OrderBy(sq => sq.QuestionOrder))
        {
            var result = session.Results.FirstOrDefault(r => r.QuestionId == sq.QuestionId);
            var isCorrect = result?.IsCorrect ?? false;

            if (isCorrect) correctCount++;

            questionResults.Add(new QuestionResultDto(
                sq.QuestionOrder,
                sq.Question.QuestionText,
                sq.Question.Answers.OrderBy(a => a.SortOrder).Select(a => new AnswerResultDto(
                    a.Id,
                    a.AnswerText,
                    a.IsCorrect,
                    a.Id == result?.SelectedAnswerId
                )).ToList(),
                result?.SelectedAnswerId,
                isCorrect,
                sq.Question.Explanation,
                sq.Question.SourceUrl,
                sq.Question.SourceTitle
            ));
        }

        return new QuizResultsDto(
            session.SessionGuid,
            session.Language.DisplayName,
            session.Category.DisplayName,
            session.DifficultyLevel.DisplayName,
            session.TotalQuestions,
            correctCount,
            session.TotalQuestions > 0 ? (double)correctCount / session.TotalQuestions * 100 : 0,
            session.StartedAt,
            session.CompletedAt,
            questionResults
        );
    }

    public async Task<int> GetStoredQuestionsCountAsync(int languageId, int categoryId, int difficultyLevelId, CancellationToken ct = default)
    {
        return await _dbContext.Questions
            .CountAsync(q => q.LanguageId == languageId
                          && q.CategoryId == categoryId
                          && q.DifficultyLevelId == difficultyLevelId, ct);
    }

    public async Task<QuizSessionDto?> RepeatQuizAsync(string originalSessionGuid, CancellationToken ct = default)
    {
        // Get the original session with its questions
        var originalSession = await _dbContext.QuizSessions
            .Include(s => s.Language)
            .Include(s => s.Category)
            .Include(s => s.DifficultyLevel)
            .Include(s => s.SessionQuestions)
            .FirstOrDefaultAsync(s => s.SessionGuid == originalSessionGuid, ct);

        if (originalSession == null)
        {
            _logger.LogWarning("Original session not found: {SessionGuid}", originalSessionGuid);
            return null;
        }

        // Create a new session with the same settings
        var newSession = new QuizSession
        {
            LanguageId = originalSession.LanguageId,
            CategoryId = originalSession.CategoryId,
            DifficultyLevelId = originalSession.DifficultyLevelId,
            Hint = originalSession.Hint,
            TotalQuestions = originalSession.TotalQuestions,
            IsOfflineGenerated = true // Mark as offline since we're reusing questions
        };

        _dbContext.QuizSessions.Add(newSession);

        // Copy the same questions to the new session (same order)
        foreach (var sq in originalSession.SessionQuestions.OrderBy(sq => sq.QuestionOrder))
        {
            newSession.SessionQuestions.Add(new QuizSessionQuestion
            {
                QuestionId = sq.QuestionId,
                QuestionOrder = sq.QuestionOrder
            });
        }

        await _dbContext.SaveChangesAsync(ct);

        return new QuizSessionDto(
            newSession.SessionGuid,
            originalSession.Language.DisplayName,
            originalSession.Category.DisplayName,
            originalSession.DifficultyLevel.DisplayName,
            originalSession.Hint,
            newSession.TotalQuestions,
            false,
            true,
            newSession.StartedAt
        );
    }
}
