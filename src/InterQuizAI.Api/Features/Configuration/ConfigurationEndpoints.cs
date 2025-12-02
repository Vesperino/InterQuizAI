namespace InterQuizAI.Api.Features.Configuration;

public static class ConfigurationEndpoints
{
    public static RouteGroupBuilder MapConfigurationEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/status", GetStatus);
        group.MapPost("/master-key", SetMasterKey);
        group.MapPost("/verify-master-key", VerifyMasterKey);
        group.MapPost("/api-key", SetApiKey);
        group.MapGet("/model", GetModel);
        group.MapPut("/model", SetModel);

        return group;
    }

    private static async Task<IResult> GetStatus(IConfigurationService service, CancellationToken ct)
    {
        var status = await service.GetStatusAsync(ct);
        return Results.Ok(status);
    }

    private static async Task<IResult> SetMasterKey(SetMasterKeyRequest request, IConfigurationService service, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(request.MasterKey) || request.MasterKey.Length < 16)
        {
            return Results.BadRequest(new { error = "Master key musi mieć minimum 16 znaków" });
        }

        var result = await service.SetMasterKeyAsync(request.MasterKey, ct);
        return result ? Results.Ok(new { message = "Master key ustawiony pomyślnie" }) : Results.BadRequest(new { error = "Nie udało się ustawić master key" });
    }

    private static async Task<IResult> VerifyMasterKey(VerifyMasterKeyRequest request, IConfigurationService service, CancellationToken ct)
    {
        var isValid = await service.VerifyMasterKeyAsync(request.MasterKey, ct);
        return Results.Ok(new { valid = isValid });
    }

    private static async Task<IResult> SetApiKey(SetApiKeyRequest request, IConfigurationService service, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(request.ApiKey))
        {
            return Results.BadRequest(new { error = "API key jest wymagany" });
        }

        var result = await service.SetApiKeyAsync(request.ApiKey, request.MasterKey, ct);
        return result ? Results.Ok(new { message = "API key zapisany pomyślnie" }) : Results.BadRequest(new { error = "Nieprawidłowy master key" });
    }

    private static async Task<IResult> GetModel(IConfigurationService service, CancellationToken ct)
    {
        var model = await service.GetModelAsync(ct);
        return Results.Ok(new { model = model ?? "gpt-4o" });
    }

    private static async Task<IResult> SetModel(SetModelRequest request, IConfigurationService service, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(request.ModelName))
        {
            return Results.BadRequest(new { error = "Nazwa modelu jest wymagana" });
        }

        await service.SetModelAsync(request.ModelName, ct);
        return Results.Ok(new { message = "Model zapisany pomyślnie" });
    }
}
