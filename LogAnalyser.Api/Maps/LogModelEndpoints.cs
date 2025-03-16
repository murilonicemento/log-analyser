using LogAnalyser.Api.Validators;
using LogAnalyser.Entities;
using LogAnalyser.Entities.Enums;
using LogAnalyser.Repositories.Contracts;
using LogAnalyser.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LogAnalyser.Api.Maps;

public static class LogModelEndpoints
{
    public static void MapLogModelEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/register",
            async ([FromBody] RegisterLogDTO registerLogDto, [FromServices] ILogRepository logRepository) =>
            {
                var validator = new RegisterLogValidator();
                var validationResult = await validator.ValidateAsync(registerLogDto);

                if (!validationResult.IsValid)
                {
                    var messages = string.Join(" | ", validationResult.Errors);

                    return Results.Problem(messages, statusCode: 400);
                }

                var log = new Log
                {
                    Timestamp = registerLogDto.Timestamp,
                    LogLevel = Enum.Parse<LogLevelOptions>(registerLogDto.LogLevel, ignoreCase: true),
                    Message = registerLogDto.Message,
                    Service = registerLogDto.Service,
                    OrigemIP = registerLogDto.OrigemIP,
                    OperationTime = registerLogDto.OperationTime,
                };

                await logRepository.RegisterLog(log);

                return Results.Created();
            });

        routes.MapGet("/api/errorsByPeriod",
            async ([FromQuery] DateTime startDate, [FromQuery] DateTime endDate,
                [FromServices] ILogRepository logRepository) =>
            {
                if (startDate >= endDate)
                {
                    return Results.Problem("Start date must be less then end date");
                }

                var logs = await logRepository.GetLogsByPeriod(startDate, endDate);

                return Results.Ok(logs);
            });
    }
}