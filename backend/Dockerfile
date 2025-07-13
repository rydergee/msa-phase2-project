# Backend Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["backend/MockMate.Api.csproj", "backend/"]
RUN dotnet restore "backend/MockMate.Api.csproj"
COPY backend/ backend/
WORKDIR "/src/backend"
RUN dotnet build "MockMate.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MockMate.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create a non-root user
RUN addgroup --system --gid 1001 mockmate && \
    adduser --system --uid 1001 --ingroup mockmate mockmate
USER mockmate

ENTRYPOINT ["dotnet", "MockMate.Api.dll"]
