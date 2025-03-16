using LogAnalyser.Entities;

namespace LogAnalyser.Repositories.Contracts;

public interface ILogRepository
{
    public Task RegisterLog(Log log);
    public Task<List<Log>> GetLogsByPeriod(DateTime startDate, DateTime endDate);
}