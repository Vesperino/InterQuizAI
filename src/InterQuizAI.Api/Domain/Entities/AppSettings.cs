namespace InterQuizAI.Api.Domain.Entities;

public class AppSettings
{
    public int Id { get; set; }
    public string MasterKeyHash { get; set; } = string.Empty;
    public string MasterKeySalt { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
