using LogAnalyser.Api.Validators;
using LogAnalyser.Entities;
using LogAnalyser.Entities.Enums;
using LogAnalyser.Repositories;
using LogAnalyser.Repositories.Contracts;
using LogAnalyser.Shared.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization;

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

        routes.MapGet("/api/logsByPeriod",
            async ([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string? logLevel,
                [FromServices] ILogRepository logRepository) =>
            {
                if (startDate > endDate)
                {
                    return Results.Problem("Start date must be less then end date", statusCode: 400);
                }

                if (logLevel is null)
                {
                    var logs = await logRepository.GetLogsByPeriod(startDate, endDate, null);

                    return Results.Ok(logs);
                }

                var filterByLogLevel =
                    Enum.TryParse(typeof(LogLevelOptions), logLevel, true, out var result);

                if (!filterByLogLevel || result is null)
                {
                    return Results.Problem("Log level is not valid.", statusCode: 400);
                }

                var logList = await logRepository.GetLogsByPeriod(startDate, endDate, (LogLevelOptions)result);

                return Results.Ok(logList);
            });

        routes.MapGet("/api/logsCountByPeriod", async ([FromQuery] DateTime startDate, [FromQuery] DateTime endDate,
            [FromServices] ILogRepository logRepository) =>
        {
            if (startDate > endDate)
            {
                return Results.Problem("Start date must be less then end date", statusCode: 400);
            }

            var logBsonDocumentList = await logRepository.GetLogsCountByPeriod(startDate, endDate);
            var logEnumerable = logBsonDocumentList
                .Select(log =>
                {
                    var obj = BsonSerializer.Deserialize<LogCountDTO>(log);

                    return new
                    {
                        Id = obj.Id.ToString(), obj.Count
                    };
                });

            return Results.Ok(logEnumerable);
        });

        routes.MapGet("/api/getAverageTimeByService",
            async ([FromQuery] string service, [FromServices] ILogRepository logRepository) =>
            {
                var averageTime = await logRepository.GetAverageTimeByService(service);

                return averageTime is null
                    ? Results.Problem("No service found.", statusCode: 404)
                    : Results.Ok(new { AverageTime = averageTime });
            });

        routes.MapGet("/api/getMostFrequentIP", async ([FromServices] ILogRepository logRepository) =>
        {
            var mostFrequentIP = await logRepository.GetMostFrequentIP();

            return Results.Ok(mostFrequentIP);
        });

        routes.MapGet("/api/getMostFrequentErrors", async ([FromServices] ILogRepository logRepository) =>
        {
            var errors = await logRepository.GetMostFrequentErrors();
            var mostFrequentErrorsDtos =
                errors.Select(error => BsonSerializer.Deserialize<MostFrequentErrorsDTO>(error));

            return Results.Ok(mostFrequentErrorsDtos);
        });
    }
}