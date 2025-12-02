using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InterQuizAI.Api.Infrastructure.OpenAI;

public class OpenAIClient : IOpenAIClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenAIClient> _logger;
    private readonly string _modelName;
    private const string ResponsesApiUrl = "https://api.openai.com/v1/responses";

    public OpenAIClient(HttpClient httpClient, ILogger<OpenAIClient> logger, string apiKey, string modelName)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        _logger = logger;
        _modelName = modelName;
    }

    public async Task<QuizGenerationResult> GenerateQuizAsync(QuizGenerationRequest request, CancellationToken ct = default)
    {
        var prompt = PromptTemplates.GetQuizGenerationPrompt(request);

        // Note: Web Search cannot be used with JSON mode, so we request text and parse JSON from it
        var requestBody = new
        {
            model = _modelName,
            input = prompt,
            tools = new[] { new { type = "web_search_preview" } }
        };

        try
        {
            _logger.LogInformation("Generating quiz for {Language}/{Category}/{Difficulty}",
                request.Language, request.Category, request.DifficultyLevel);

            var response = await _httpClient.PostAsJsonAsync(ResponsesApiUrl, requestBody, ct);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(ct);
                _logger.LogError("OpenAI API error: {StatusCode} - {Content}", response.StatusCode, errorContent);
                return new QuizGenerationResult(false, new List<GeneratedQuestion>(), $"API Error: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync(ct);
            _logger.LogDebug("OpenAI Response: {Response}", responseContent);

            var result = ParseQuizResponse(responseContent);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate quiz");
            return new QuizGenerationResult(false, new List<GeneratedQuestion>(), ex.Message);
        }
    }

    private QuizGenerationResult ParseQuizResponse(string responseContent)
    {
        try
        {
            using var doc = JsonDocument.Parse(responseContent);
            var root = doc.RootElement;

            // Navigate through the Responses API structure to find the text output
            string? jsonContent = null;

            if (root.TryGetProperty("output", out var output))
            {
                foreach (var item in output.EnumerateArray())
                {
                    if (item.TryGetProperty("type", out var type) && type.GetString() == "message")
                    {
                        if (item.TryGetProperty("content", out var content))
                        {
                            foreach (var contentItem in content.EnumerateArray())
                            {
                                if (contentItem.TryGetProperty("type", out var contentType) &&
                                    contentType.GetString() == "output_text")
                                {
                                    if (contentItem.TryGetProperty("text", out var text))
                                    {
                                        jsonContent = text.GetString();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(jsonContent))
            {
                _logger.LogError("Could not find text content in response");
                return new QuizGenerationResult(false, new List<GeneratedQuestion>(), "Invalid response format");
            }

            // Extract JSON from text (it may be wrapped in markdown code blocks)
            jsonContent = ExtractJsonFromText(jsonContent);

            if (string.IsNullOrEmpty(jsonContent))
            {
                _logger.LogError("Could not extract JSON from response text");
                return new QuizGenerationResult(false, new List<GeneratedQuestion>(), "Could not extract JSON from response");
            }

            // Parse the quiz JSON
            var quizResponse = JsonSerializer.Deserialize<QuizJsonResponse>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (quizResponse?.Questions == null || quizResponse.Questions.Count == 0)
            {
                return new QuizGenerationResult(false, new List<GeneratedQuestion>(), "No questions in response");
            }

            var questions = quizResponse.Questions.Select(q => new GeneratedQuestion(
                q.QuestionText,
                q.Answers.Select(a => new GeneratedAnswer(a.Text, a.IsCorrect)).ToList(),
                q.Explanation,
                q.SourceUrl,
                q.SourceTitle
            )).ToList();

            _logger.LogInformation("Generated {Count} questions", questions.Count);

            return new QuizGenerationResult(true, questions, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse quiz response");
            return new QuizGenerationResult(false, new List<GeneratedQuestion>(), $"Parse error: {ex.Message}");
        }
    }

    private static string? ExtractJsonFromText(string text)
    {
        // Try to find JSON in the text (may be wrapped in markdown code blocks)
        var jsonStart = text.IndexOf('{');
        var jsonEnd = text.LastIndexOf('}');

        if (jsonStart >= 0 && jsonEnd > jsonStart)
        {
            return text.Substring(jsonStart, jsonEnd - jsonStart + 1);
        }

        return null;
    }

    private class QuizJsonResponse
    {
        [JsonPropertyName("questions")]
        public List<QuestionJson> Questions { get; set; } = new();
    }

    private class QuestionJson
    {
        [JsonPropertyName("questionText")]
        public string QuestionText { get; set; } = string.Empty;

        [JsonPropertyName("answers")]
        public List<AnswerJson> Answers { get; set; } = new();

        [JsonPropertyName("explanation")]
        public string? Explanation { get; set; }

        [JsonPropertyName("sourceUrl")]
        public string? SourceUrl { get; set; }

        [JsonPropertyName("sourceTitle")]
        public string? SourceTitle { get; set; }
    }

    private class AnswerJson
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        [JsonPropertyName("isCorrect")]
        public bool IsCorrect { get; set; }
    }
}
