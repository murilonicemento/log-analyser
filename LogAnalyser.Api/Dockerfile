FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

USER $APP_UID

WORKDIR /app

EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["LogAnalyser.Api/LogAnalyser.Api.csproj", "LogAnalyser.Api/"]
COPY ["LogAnalyser.Entities/LogAnalyser.Entities.csproj", "LogAnalyser.Entities/"]
COPY ["LogAnalyser.Persistence/LogAnalyser.Persistence.csproj", "LogAnalyser.Persistence/"]
COPY ["LogAnalyser.Persistence.Contracts/LogAnalyser.Persistence.Contracts.csproj", "LogAnalyser.Persistence.Contracts/"]
COPY ["LogAnalyser.Repositories/LogAnalyser.Repositories.csproj", "LogAnalyser.Repositories/"]
COPY ["LogAnalyser.Repositories.Contracts/LogAnalyser.Repositories.Contracts.csproj", "LogAnalyser.Repositories.Contracts/"]
COPY ["LogAnalyser.Shared/LogAnalyser.Shared.csproj", "LogAnalyser.Shared/"]

RUN dotnet restore "LogAnalyser.Api/LogAnalyser.Api.csproj"

COPY . .

WORKDIR "/src/LogAnalyser.Api"

RUN dotnet build "LogAnalyser.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "LogAnalyser.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base as final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT [ "dotnet", "LogAnalyser.Api.dll" ]