namespace InterQuizAI.Api.Domain.Entities;

public class DifficultyLevel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SortOrder { get; set; }

    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<QuizSession> QuizSessions { get; set; } = new List<QuizSession>();
}
