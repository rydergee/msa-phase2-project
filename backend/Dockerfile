# Backend Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MockMate.Api.csproj", "./"]
RUN dotnet restore "MockMate.Api.csproj"
COPY . .
RUN dotnet build "MockMate.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MockMate.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create a non-root user
RUN addgroup --system --gid 1001 mockmate && \
    adduser --system --uid 1001 --ingroup mockmate mockmate

# Change ownership of the app directory
RUN chown -R mockmate:mockmate /app
USER mockmate

ENTRYPOINT ["dotnet", "MockMate.Api.dll"]
