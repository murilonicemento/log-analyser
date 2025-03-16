using LogAnalyser.Entities.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace LogAnalyser.Shared.DTOs;

public class LogCountDTO
{
    public LogLevelOptions Id { get; set; }

    public int Count { get; set; }
}