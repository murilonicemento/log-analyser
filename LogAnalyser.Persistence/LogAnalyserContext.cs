using Microsoft.Extensions.Options;
using LogAnalyser.Entities;
using LogAnalyser.Persistence.Configuration;
using LogAnalyser.Persistence.Contracts;
using MongoDB.Driver;

namespace LogAnalyser.Persistence;

public class LogAnalyserContext : ILogAnalyserContext
{
    public IMongoCollection<Log> Log { get; }
    private readonly IMongoDatabase _database;

    public LogAnalyserContext(IOptions<LogAnalyserConfiguration> configuration)
    {
        var client = new MongoClient(configuration.Value.ConnectionString);
        _database = client.GetDatabase(configuration.Value.Database);
        Log = _database.GetCollection<Log>("log");
    }
}