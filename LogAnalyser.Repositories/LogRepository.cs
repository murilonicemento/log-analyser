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
}