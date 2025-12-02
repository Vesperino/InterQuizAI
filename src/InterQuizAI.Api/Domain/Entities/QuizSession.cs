namespace InterQuizAI.Api.Domain.Entities;

public class QuizSession
{
    public int Id { get; set; }
    public string SessionGuid { get; set; } = Guid.NewGuid().ToString();
    public int LanguageId { get; set; }
    public int CategoryId { get; set; }
    public int DifficultyLevelId { get; set; }
    public string? Hint { get; set; }
    public int TotalQuestions { get; set; } = 20;
    public bool IsCompleted { get; set; } = false;
    public bool IsOfflineGenerated { get; set; } = false;
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    public ProgrammingLanguage Language { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public DifficultyLevel DifficultyLevel { get; set; } = null!;
    public ICollection<QuizSessionQuestion> SessionQuestions { get; set; } = new List<QuizSessionQuestion>();
    public ICollection<QuizResult> Results { get; set; } = new List<QuizResult>();
}
