namespace LogAnalyser.Shared.DTOs;

public class RegisterLogDTO
{
    public DateTime Timestamp { get; set; }

    public string LogLevel { get; set; }

    public string Message { get; set; }

    public string Service { get; set; }

    public string OrigemIP { get; set; }

    public int OperationTime { get; set; }
}