namespace InterQuizAI.Api.Domain.Entities;

public class QuizSessionQuestion
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public int QuestionId { get; set; }
    public int QuestionOrder { get; set; }

    public QuizSession Session { get; set; } = null!;
    public Question Question { get; set; } = null!;
}
