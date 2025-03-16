using LogAnalyser.Entities;
using LogAnalyser.Persistence.Contracts;
using LogAnalyser.Repositories.Contracts;
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

    public async Task<List<Log>> GetLogsByPeriod(DateTime startDate, DateTime endDate)
    {
        var filter = Builders<Log>.Filter.And(
            Builders<Log>.Filter.Gte(log => log.Timestamp, startDate),
            Builders<Log>.Filter.Lte(log => log.Timestamp, endDate)
        );

        return await _logAnalyserContext.Log.Find(filter).ToListAsync();
    }
}