using InterQuizAI.Api.Domain.Entities;
using InterQuizAI.Api.Infrastructure.Persistence;
using InterQuizAI.Api.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace InterQuizAI.Api.Features.Configuration;

public interface IConfigurationService
{
    Task<ConfigStatusResponse> GetStatusAsync(CancellationToken ct = default);
    Task<bool> SetMasterKeyAsync(string masterKey, CancellationToken ct = default);
    Task<bool> VerifyMasterKeyAsync(string masterKey, CancellationToken ct = default);
    Task<bool> SetApiKeyAsync(string apiKey, string masterKey, CancellationToken ct = default);
    Task<bool> SetModelAsync(string modelName, CancellationToken ct = default);
    Task<string?> GetModelAsync(CancellationToken ct = default);
    Task<string?> GetDecryptedApiKeyAsync(string masterKey, CancellationToken ct = default);
}

public class ConfigurationService : IConfigurationService
{
    private readonly InterQuizDbContext _dbContext;
    private readonly IEncryptionService _encryptionService;

    public ConfigurationService(InterQuizDbContext dbContext, IEncryptionService encryptionService)
    {
        _dbContext = dbContext;
        _encryptionService = encryptionService;
    }

    public async Task<ConfigStatusResponse> GetStatusAsync(CancellationToken ct = default)
    {
        var appSettings = await _dbContext.AppSettings.FirstOrDefaultAsync(ct);
        var apiConfig = await _dbContext.ApiConfigurations.FirstOrDefaultAsync(ct);

        return new ConfigStatusResponse(
            IsMasterKeySet: appSettings != null && !string.IsNullOrEmpty(appSettings.MasterKeyHash),
            IsApiKeySet: apiConfig != null && !string.IsNullOrEmpty(apiConfig.EncryptedApiKey),
            ModelName: apiConfig?.ModelName
        );
    }

    public async Task<bool> SetMasterKeyAsync(string masterKey, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(masterKey) || masterKey.Length < 16)
            return false;

        var existing = await _dbContext.AppSettings.FirstOrDefaultAsync(ct);

        var salt = _encryptionService.GenerateSalt();
        var hash = _encryptionService.HashMasterKey(masterKey, salt);

        if (existing != null)
        {
            existing.MasterKeyHash = hash;
            existing.MasterKeySalt = salt;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            _dbContext.AppSettings.Add(new AppSettings
            {
                MasterKeyHash = hash,
                MasterKeySalt = salt
            });
        }

        await _dbContext.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> VerifyMasterKeyAsync(string masterKey, CancellationToken ct = default)
    {
        var appSettings = await _dbContext.AppSettings.FirstOrDefaultAsync(ct);
        if (appSettings == null)
            return false;

        return _encryptionService.VerifyMasterKey(masterKey, appSettings.MasterKeyHash, appSettings.MasterKeySalt);
    }

    public async Task<bool> SetApiKeyAsync(string apiKey, string masterKey, CancellationToken ct = default)
    {
        if (!await VerifyMasterKeyAsync(masterKey, ct))
            return false;

        var encrypted = _encryptionService.Encrypt(apiKey, masterKey);
        var existing = await _dbContext.ApiConfigurations.FirstOrDefaultAsync(ct);

        if (existing != null)
        {
            existing.EncryptedApiKey = encrypted.CipherText;
            existing.Salt = encrypted.Salt;
            existing.IV = encrypted.IV;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            _dbContext.ApiConfigurations.Add(new ApiConfiguration
            {
                EncryptedApiKey = encrypted.CipherText,
                Salt = encrypted.Salt,
                IV = encrypted.IV
            });
        }

        await _dbContext.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> SetModelAsync(string modelName, CancellationToken ct = default)
    {
        var existing = await _dbContext.ApiConfigurations.FirstOrDefaultAsync(ct);

        if (existing != null)
        {
            existing.ModelName = modelName;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            _dbContext.ApiConfigurations.Add(new ApiConfiguration
            {
                ModelName = modelName
            });
        }

        await _dbContext.SaveChangesAsync(ct);
        return true;
    }

    public async Task<string?> GetModelAsync(CancellationToken ct = default)
    {
        var config = await _dbContext.ApiConfigurations.FirstOrDefaultAsync(ct);
        return config?.ModelName;
    }

    public async Task<string?> GetDecryptedApiKeyAsync(string masterKey, CancellationToken ct = default)
    {
        if (!await VerifyMasterKeyAsync(masterKey, ct))
            return null;

        var config = await _dbContext.ApiConfigurations.FirstOrDefaultAsync(ct);
        if (config == null || string.IsNullOrEmpty(config.EncryptedApiKey))
            return null;

        return _encryptionService.Decrypt(config.EncryptedApiKey, config.Salt, config.IV, masterKey);
    }
}
