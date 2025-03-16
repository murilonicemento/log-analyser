using LogAnalyser.Entities;
using MongoDB.Driver;

namespace LogAnalyser.Persistence.Contracts;

public interface ILogAnalyserContext
{
    public IMongoCollection<Log> Log { get; }
}