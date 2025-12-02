namespace InterQuizAI.Api.Features.Languages;

public record LanguageDto(int Id, string Name, string DisplayName, string TechnologyType, bool IsCustom);
public record CategoryDto(int Id, string Name, string DisplayName, string? Description, bool AllowsHint);
public record DifficultyLevelDto(int Id, string Name, string DisplayName, string? Description, int SortOrder);
public record TechnologyTypeDto(int Id, string Name, string DisplayName);
public record AddLanguageRequest(string Name, string DisplayName, string TechnologyType);
