using InterQuizAI.Api.Domain.Entities;
using InterQuizAI.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InterQuizAI.Api.Features.Languages;

public interface ILanguagesService
{
    Task<List<TechnologyTypeDto>> GetTechnologyTypesAsync(CancellationToken ct = default);
    Task<List<LanguageDto>> GetLanguagesAsync(string? technologyType = null, CancellationToken ct = default);
    Task<List<CategoryDto>> GetCategoriesAsync(string technologyType, CancellationToken ct = default);
    Task<List<DifficultyLevelDto>> GetDifficultyLevelsAsync(CancellationToken ct = default);
    Task<LanguageDto?> AddLanguageAsync(AddLanguageRequest request, CancellationToken ct = default);
    Task<bool> DeleteLanguageAsync(int id, CancellationToken ct = default);
}

public class LanguagesService : ILanguagesService
{
    private readonly InterQuizDbContext _dbContext;

    public LanguagesService(InterQuizDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TechnologyTypeDto>> GetTechnologyTypesAsync(CancellationToken ct = default)
    {
        return await _dbContext.TechnologyTypes
            .Select(t => new TechnologyTypeDto(t.Id, t.Name, t.DisplayName))
            .ToListAsync(ct);
    }

    public async Task<List<LanguageDto>> GetLanguagesAsync(string? technologyType = null, CancellationToken ct = default)
    {
        var query = _dbContext.ProgrammingLanguages
            .Include(l => l.TechnologyType)
            .Where(l => l.IsActive);

        if (!string.IsNullOrEmpty(technologyType))
        {
            query = query.Where(l => l.TechnologyType.Name == technologyType);
        }

        return await query
            .Select(l => new LanguageDto(l.Id, l.Name, l.DisplayName, l.TechnologyType.DisplayName, l.IsCustom))
            .ToListAsync(ct);
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync(string technologyType, CancellationToken ct = default)
    {
        return await _dbContext.Categories
            .Include(c => c.TechnologyType)
            .Where(c => c.IsActive && c.TechnologyType.Name == technologyType)
            .Select(c => new CategoryDto(c.Id, c.Name, c.DisplayName, c.Description, c.AllowsHint))
            .ToListAsync(ct);
    }

    public async Task<List<DifficultyLevelDto>> GetDifficultyLevelsAsync(CancellationToken ct = default)
    {
        return await _dbContext.DifficultyLevels
            .OrderBy(d => d.SortOrder)
            .Select(d => new DifficultyLevelDto(d.Id, d.Name, d.DisplayName, d.Description, d.SortOrder))
            .ToListAsync(ct);
    }

    public async Task<LanguageDto?> AddLanguageAsync(AddLanguageRequest request, CancellationToken ct = default)
    {
        var techType = await _dbContext.TechnologyTypes
            .FirstOrDefaultAsync(t => t.Name == request.TechnologyType, ct);

        if (techType == null)
            return null;

        var existing = await _dbContext.ProgrammingLanguages
            .AnyAsync(l => l.Name == request.Name, ct);

        if (existing)
            return null;

        var language = new ProgrammingLanguage
        {
            Name = request.Name.ToLower().Replace(" ", "_"),
            DisplayName = request.DisplayName,
            TechnologyTypeId = techType.Id,
            IsCustom = true,
            IsActive = true
        };

        _dbContext.ProgrammingLanguages.Add(language);
        await _dbContext.SaveChangesAsync(ct);

        return new LanguageDto(language.Id, language.Name, language.DisplayName, techType.DisplayName, true);
    }

    public async Task<bool> DeleteLanguageAsync(int id, CancellationToken ct = default)
    {
        var language = await _dbContext.ProgrammingLanguages
            .FirstOrDefaultAsync(l => l.Id == id && l.IsCustom, ct);

        if (language == null)
            return false;

        _dbContext.ProgrammingLanguages.Remove(language);
        await _dbContext.SaveChangesAsync(ct);
        return true;
    }
}
