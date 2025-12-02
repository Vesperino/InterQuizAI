namespace InterQuizAI.Api.Domain.Entities;

public class QuizResult
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public int QuestionId { get; set; }
    public int? SelectedAnswerId { get; set; }
    public bool IsCorrect { get; set; } = false;
    public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;

    public QuizSession Session { get; set; } = null!;
    public Question Question { get; set; } = null!;
    public Answer? SelectedAnswer { get; set; }
}
