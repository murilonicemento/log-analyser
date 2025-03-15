using LogAnalyser.PersistenceContracts;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace LogAnalyser.Api.Maps;

public static class LogModelEndpoints
{
    public static void MapLogModelEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/logs",
            async ([FromServices] ILogAnalyserContext logAnalyserContext) =>
            {
                return await logAnalyserContext.Log.Find(_ => true).ToListAsync();
            });
    }
}