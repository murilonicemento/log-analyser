using LogAnalyser.Entities;
using MongoDB.Driver;

namespace LogAnalyser.PersistenceContracts;

public interface ILogAnalyserContext
{
    public IMongoCollection<Log> Log { get; }
}