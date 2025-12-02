namespace InterQuizAI.Api.Domain.Entities;

public class Question
{
    public int Id { get; set; }
    public string ExternalId { get; set; } = Guid.NewGuid().ToString();
    public int LanguageId { get; set; }
    public int CategoryId { get; set; }
    public int DifficultyLevelId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string? Explanation { get; set; }
    public string? SourceUrl { get; set; }
    public string? SourceTitle { get; set; }
    public string? Hint { get; set; }
    public bool IsVerified { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ProgrammingLanguage Language { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public DifficultyLevel DifficultyLevel { get; set; } = null!;
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    public ICollection<QuizSessionQuestion> SessionQuestions { get; set; } = new List<QuizSessionQuestion>();
    public ICollection<QuizResult> Results { get; set; } = new List<QuizResult>();
}
