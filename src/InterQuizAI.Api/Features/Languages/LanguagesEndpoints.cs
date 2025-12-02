namespace InterQuizAI.Api.Features.Languages;

public static class LanguagesEndpoints
{
    public static RouteGroupBuilder MapLanguagesEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/technology-types", GetTechnologyTypes);
        group.MapGet("/", GetLanguages);
        group.MapGet("/{type}", GetLanguagesByType);
        group.MapPost("/", AddLanguage);
        group.MapDelete("/{id:int}", DeleteLanguage);
        group.MapGet("/categories/{type}", GetCategories);
        group.MapGet("/difficulty-levels", GetDifficultyLevels);

        return group;
    }

    private static async Task<IResult> GetTechnologyTypes(ILanguagesService service, CancellationToken ct)
    {
        var types = await service.GetTechnologyTypesAsync(ct);
        return Results.Ok(types);
    }

    private static async Task<IResult> GetLanguages(ILanguagesService service, CancellationToken ct)
    {
        var languages = await service.GetLanguagesAsync(ct: ct);
        return Results.Ok(languages);
    }

    private static async Task<IResult> GetLanguagesByType(string type, ILanguagesService service, CancellationToken ct)
    {
        var languages = await service.GetLanguagesAsync(type, ct);
        return Results.Ok(languages);
    }

    private static async Task<IResult> AddLanguage(AddLanguageRequest request, ILanguagesService service, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.DisplayName))
        {
            return Results.BadRequest(new { error = "Nazwa i wyświetlana nazwa są wymagane" });
        }

        var result = await service.AddLanguageAsync(request, ct);
        return result != null
            ? Results.Created($"/api/languages/{result.Id}", result)
            : Results.BadRequest(new { error = "Nie udało się dodać języka. Sprawdź czy typ technologii jest poprawny i czy język nie istnieje." });
    }

    private static async Task<IResult> DeleteLanguage(int id, ILanguagesService service, CancellationToken ct)
    {
        var result = await service.DeleteLanguageAsync(id, ct);
        return result
            ? Results.Ok(new { message = "Język usunięty pomyślnie" })
            : Results.NotFound(new { error = "Nie znaleziono języka lub język nie jest niestandardowy" });
    }

    private static async Task<IResult> GetCategories(string type, ILanguagesService service, CancellationToken ct)
    {
        var categories = await service.GetCategoriesAsync(type, ct);
        return Results.Ok(categories);
    }

    private static async Task<IResult> GetDifficultyLevels(ILanguagesService service, CancellationToken ct)
    {
        var levels = await service.GetDifficultyLevelsAsync(ct);
        return Results.Ok(levels);
    }
}
