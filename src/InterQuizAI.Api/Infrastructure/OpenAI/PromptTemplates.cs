namespace InterQuizAI.Api.Infrastructure.OpenAI;

public static class PromptTemplates
{
    public static string GetQuizGenerationPrompt(QuizGenerationRequest request)
    {
        var hintText = !string.IsNullOrEmpty(request.Hint)
            ? $"\nDODATKOWA WSKAZÓWKA: Skup się szczególnie na: {request.Hint}"
            : "";

        return $@"Jesteś ekspertem technicznym tworzącym quiz do przygotowania na rozmowę kwalifikacyjną.

ZADANIE: Wygeneruj dokładnie {request.QuestionCount} pytań wielokrotnego wyboru dla programisty {request.LanguageDisplayName}.

KONTEKST:
- Technologia: {request.LanguageDisplayName}
- Kategoria: {request.Category} - {request.CategoryDescription}
- Poziom trudności: {request.DifficultyLevel} - {request.DifficultyDescription}
{hintText}

WYMAGANIA:
1. Każde pytanie MUSI mieć dokładnie 5 odpowiedzi (A do E)
2. Dokładnie JEDNA odpowiedź musi być prawidłowa
3. Nieprawidłowe odpowiedzi powinny być wiarygodne, ale wyraźnie błędne dla kogoś znającego temat
4. Pytania muszą być praktyczne i istotne dla prawdziwych rozmów kwalifikacyjnych
5. Unikaj banalnych pytań - skup się na zrozumieniu, nie na zapamiętywaniu
6. UŻYJ WEB SEARCH aby zweryfikować poprawność techniczną pytań i odpowiedzi
7. Dla każdego pytania podaj wyjaśnienie DLACZEGO dana odpowiedź jest prawidłowa oraz źródło

FORMAT WYJŚCIA (JSON):
{{
  ""questions"": [
    {{
      ""questionText"": ""Treść pytania?"",
      ""answers"": [
        {{ ""text"": ""Odpowiedź A"", ""isCorrect"": false }},
        {{ ""text"": ""Odpowiedź B"", ""isCorrect"": true }},
        {{ ""text"": ""Odpowiedź C"", ""isCorrect"": false }},
        {{ ""text"": ""Odpowiedź D"", ""isCorrect"": false }},
        {{ ""text"": ""Odpowiedź E"", ""isCorrect"": false }}
      ],
      ""explanation"": ""Szczegółowe wyjaśnienie dlaczego odpowiedź B jest prawidłowa (100-200 słów)"",
      ""sourceUrl"": ""https://docs.microsoft.com/..."",
      ""sourceTitle"": ""Oficjalna dokumentacja Microsoft""
    }}
  ]
}}

KONTROLA JAKOŚCI:
- Zweryfikuj każdą odpowiedź technicznie używając web search
- Upewnij się że pytania testują zrozumienie, nie tylko pamięć
- Dopasuj trudność do określonego poziomu
- Używaj aktualnych informacji i najlepszych praktyk
- Wszystkie wyjaśnienia i źródła MUSZĄ być w języku polskim lub angielskim (preferowany angielski dla źródeł technicznych)

Wygeneruj quiz teraz:";
    }
}
