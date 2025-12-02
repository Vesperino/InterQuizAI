namespace InterQuizAI.Api.Domain.Entities;

public class ApiConfiguration
{
    public int Id { get; set; }
    public string EncryptedApiKey { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public string IV { get; set; } = string.Empty;
    public string ModelName { get; set; } = "gpt-4o";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
