using LogAnalyser.Entities.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LogAnalyser.Entities;

public class Log
{
    [BsonId] public ObjectId InternalId { get; set; }
    public DateTime Timestamp { get; set; }
    public LogLevelOptions LogLevel { get; set; }
    public string Message { get; set; }
    public string Service { get; set; }
    public string OrigemIP { get; set; }
    public int OperationTime { get; set; }
}