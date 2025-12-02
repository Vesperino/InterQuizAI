namespace InterQuizAI.Api.Domain.Entities;

public class ProgrammingLanguage
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public int TechnologyTypeId { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsCustom { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public TechnologyType TechnologyType { get; set; } = null!;
    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<QuizSession> QuizSessions { get; set; } = new List<QuizSession>();
}
