namespace InterQuizAI.Api.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int TechnologyTypeId { get; set; }
    public bool AllowsHint { get; set; } = false;
    public bool IsActive { get; set; } = true;

    public TechnologyType TechnologyType { get; set; } = null!;
    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<QuizSession> QuizSessions { get; set; } = new List<QuizSession>();
}
