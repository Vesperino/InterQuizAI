namespace InterQuizAI.Api.Domain.Entities;

public class Answer
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string AnswerText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; } = false;
    public int SortOrder { get; set; }

    public Question Question { get; set; } = null!;
    public ICollection<QuizResult> Results { get; set; } = new List<QuizResult>();
}
