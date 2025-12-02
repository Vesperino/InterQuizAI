namespace InterQuizAI.Api.Domain.Entities;

public class TechnologyType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;

    public ICollection<ProgrammingLanguage> Languages { get; set; } = new List<ProgrammingLanguage>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
}
