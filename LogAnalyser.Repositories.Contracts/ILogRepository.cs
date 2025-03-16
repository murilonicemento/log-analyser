using LogAnalyser.Entities;
using LogAnalyser.Entities.Enums;
using MongoDB.Bson;

namespace LogAnalyser.Repositories.Contracts;

public interface ILogRepository
{
    public Task RegisterLog(Log log);
    public Task<List<Log>> GetLogsByPeriod(DateTime startDate, DateTime endDate, LogLevelOptions? logLevel);

    public Task<List<BsonDocument>> GetLogsCountByPeriod(DateTime startDate, DateTime endDate);
}