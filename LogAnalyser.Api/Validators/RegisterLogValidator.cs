using FluentValidation;
using LogAnalyser.Entities.Enums;
using LogAnalyser.Shared.DTOs;

namespace LogAnalyser.Api.Validators;

public class RegisterLogValidator : AbstractValidator<RegisterLogDTO>
{
    public RegisterLogValidator()
    {
        RuleFor(registerLogDto => registerLogDto.Timestamp).NotNull();
        RuleFor(registerLogDto => registerLogDto.LogLevel)
            .NotNull()
            .IsEnumName(typeof(LogLevelOptions), caseSensitive: false)
            .WithMessage("Log level is not valid.");
        RuleFor(registerLogDto => registerLogDto.Message).NotNull();
        RuleFor(registerLogDto => registerLogDto.Service).NotNull();
        RuleFor(registerLogDto => registerLogDto.OrigemIP).NotNull();
        RuleFor(registerLogDto => registerLogDto.OperationTime).NotNull().GreaterThanOrEqualTo(1);
    }
}