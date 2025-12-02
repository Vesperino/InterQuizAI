namespace InterQuizAI.Api.Features.History;

public static class HistoryEndpoints
{
    public static RouteGroupBuilder MapHistoryEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetHistory);
        group.MapGet("/stats", GetStats);
        group.MapGet("/{sessionGuid}", GetHistoryItem);
        group.MapDelete("/{sessionGuid}", DeleteHistoryItem);

        return group;
    }

    private static async Task<IResult> GetHistory(IHistoryService service, CancellationToken ct)
    {
        var history = await service.GetHistoryAsync(ct);
        return Results.Ok(history);
    }

    private static async Task<IResult> GetStats(IHistoryService service, CancellationToken ct)
    {
        var stats = await service.GetStatsAsync(ct);
        return Results.Ok(stats);
    }

    private static async Task<IResult> GetHistoryItem(string sessionGuid, IHistoryService service, CancellationToken ct)
    {
        var item = await service.GetHistoryItemAsync(sessionGuid, ct);
        return item != null ? Results.Ok(item) : Results.NotFound();
    }

    private static async Task<IResult> DeleteHistoryItem(string sessionGuid, IHistoryService service, CancellationToken ct)
    {
        var result = await service.DeleteHistoryItemAsync(sessionGuid, ct);
        return result ? Results.Ok(new { message = "Usunięto pomyślnie" }) : Results.NotFound();
    }
}
