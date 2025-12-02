namespace InterQuizAI.Api.Infrastructure.OpenAI;

public static class PromptTemplates
{
    public static string GetQuizGenerationPrompt(QuizGenerationRequest request)
    {
        var outputLanguage = request.QuizLanguage?.ToLower() == "pl" ? "Polish" : "English";
        var hintText = !string.IsNullOrEmpty(request.Hint)
            ? $"\nADDITIONAL FOCUS: Emphasize questions about: {request.Hint}"
            : "";

        return $@"You are a technical expert creating a quiz for job interview preparation.

TASK: Generate exactly {request.QuestionCount} multiple choice questions for a {request.LanguageDisplayName} developer.

CRITICAL REQUIREMENTS:
- The ""questions"" array MUST contain EXACTLY {request.QuestionCount} elements
- OUTPUT LANGUAGE: All questions, answers and explanations MUST be in {outputLanguage}

CONTEXT:
- Technology: {request.LanguageDisplayName}
- Category: {request.Category} - {request.CategoryDescription}
- Difficulty: {request.DifficultyLevel} - {request.DifficultyDescription}
{hintText}

CODE FORMATTING (IMPORTANT):
- When including code snippets, use markdown code blocks with language identifier
- Format: ```csharp\ncode here\n``` or ```javascript\ncode here\n```
- Use inline code with backticks for short references: `ClassName` or `methodName()`
- Code in questions and answers should be properly formatted for readability

REQUIREMENTS:
1. Each question MUST have exactly 5 answers (A to E)
2. Exactly ONE answer must be correct
3. Incorrect answers should be plausible but clearly wrong for experts
4. Questions must be practical for real job interviews
5. Focus on understanding, not memorization
6. USE WEB SEARCH to verify technical correctness
7. Include detailed explanation (100-200 words) with source URL

OUTPUT FORMAT (JSON):
{{
  ""questions"": [
    {{
      ""questionText"": ""Question with ```csharp\ncode\n``` if needed"",
      ""answers"": [
        {{ ""text"": ""Answer A with `code` if needed"", ""isCorrect"": false }},
        {{ ""text"": ""Answer B"", ""isCorrect"": true }},
        {{ ""text"": ""Answer C"", ""isCorrect"": false }},
        {{ ""text"": ""Answer D"", ""isCorrect"": false }},
        {{ ""text"": ""Answer E"", ""isCorrect"": false }}
      ],
      ""explanation"": ""Detailed explanation with ```csharp\nexample code\n``` if helpful"",
      ""sourceUrl"": ""https://docs.example.com/..."",
      ""sourceTitle"": ""Source Title""
    }}
  ]
}}

BEFORE FINISHING: Count questions - must be exactly {request.QuestionCount}!

Generate quiz now:";
    }
}
