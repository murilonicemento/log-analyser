using LogAnalyser.Api.Maps;
using LogAnalyser.Persistence;
using LogAnalyser.Persistence.Configuration;
using LogAnalyser.PersistenceContracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<LogAnalyserConfiguration>(builder.Configuration.GetSection("LogAnalyserConfiguration"));

builder.Services.AddSingleton<ILogAnalyserContext, LogAnalyserContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapLogModelEndpoints();

app.Run();