using LogAnalyser.Entities;
using LogAnalyser.Entities.Enums;
using LogAnalyser.Persistence.Contracts;
using LogAnalyser.Repositories.Contracts;
using LogAnalyser.Shared.DTOs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace LogAnalyser.Repositories;

public class LogRepository : ILogRepository
{
    private readonly ILogAnalyserContext _logAnalyserContext;

    public LogRepository(ILogAnalyserContext logAnalyserContext)
    {
        _logAnalyserContext = logAnalyserContext;
    }

    public async Task RegisterLog(Log log)
    {
        await _logAnalyserContext.Log.InsertOneAsync(log);
    }

    public async Task<List<Log>> GetLogsByPeriod(DateTime startDate, DateTime endDate, LogLevelOptions? logLevel)
    {
        var filter = logLevel is not null
            ? Builders<Log>.Filter.And(
                Builders<Log>.Filter.Gte(log => log.Timestamp, startDate),
                Builders<Log>.Filter.Lte(log => log.Timestamp, endDate),
                Builders<Log>.Filter.Eq(log => log.LogLevel, logLevel)
            )
            : Builders<Log>.Filter.And(
                Builders<Log>.Filter.Gte(log => log.Timestamp, startDate),
                Builders<Log>.Filter.Lte(log => log.Timestamp, endDate)
            );

        return await _logAnalyserContext.Log.Find(filter).ToListAsync();
    }

    public async Task<List<BsonDocument>> GetLogsCountByPeriod(DateTime startDate, DateTime endDate)
    {
        var pipeline = new[]
        {
            new BsonDocument("$match", new BsonDocument("Timestamp", new BsonDocument
            {
                { "$gte", startDate },
                { "$lte", endDate },
            })),
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", "$LogLevel" },
                { "Count", new BsonDocument("$sum", 1) }
            })
        };

        return await _logAnalyserContext.Log.Aggregate<BsonDocument>(pipeline).ToListAsync();
    }

    public async Task<double?> GetAverageTimeByService(string service)
    {
        var pipeline = new[]
        {
            new BsonDocument("$match", new BsonDocument("Service", new BsonDocument("$eq", service))),
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", BsonNull.Value },
                { "AverageTime", new BsonDocument("$avg", "$OperationTime") }
            })
        };

        var result = await _logAnalyserContext.Log
            .Aggregate<BsonDocument>(pipeline)
            .FirstOrDefaultAsync();

        return result?["AverageTime"].ToDouble();
    }

    public async Task<string> GetMostFrequentIP()
    {
        var pipeline = new[]
        {
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", "$OrigemIP" },
                { "Count", new BsonDocument("$sum", 1) }
            }),
            new BsonDocument("$sort", new BsonDocument("Count", -1)),
            new BsonDocument("$limit", 1)
        };

        var result = await _logAnalyserContext.Log
            .Aggregate<BsonDocument>(pipeline)
            .FirstOrDefaultAsync();

        return result?["_id"].ToString() ?? string.Empty;
    }
}