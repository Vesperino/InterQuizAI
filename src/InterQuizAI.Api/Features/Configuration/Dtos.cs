namespace InterQuizAI.Api.Features.Configuration;

public record SetMasterKeyRequest(string MasterKey);
public record VerifyMasterKeyRequest(string MasterKey);
public record SetApiKeyRequest(string ApiKey, string MasterKey);
public record SetModelRequest(string ModelName);

public record ConfigStatusResponse(
    bool IsMasterKeySet,
    bool IsApiKeySet,
    string? ModelName
);
