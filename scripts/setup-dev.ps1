# MockMate Development Setup Script for Windows

Write-Host "🛠️ Setting up MockMate Development Environment..." -ForegroundColor Green

# Check if Docker is running
try {
    docker info | Out-Null
    Write-Host "✅ Docker is running" -ForegroundColor Green
} catch {
    Write-Host "❌ Docker is not running. Please start Docker and try again." -ForegroundColor Red
    exit 1
}

# Stop any existing containers
Write-Host "🛑 Stopping any existing containers..." -ForegroundColor Yellow
docker-compose -f docker-compose.dev.yml down

# Build and start development containers
Write-Host "🔨 Building and starting development containers..." -ForegroundColor Blue
docker-compose -f docker-compose.dev.yml up --build -d

# Wait for services to be ready
Write-Host "⏳ Waiting for services to be ready..." -ForegroundColor Yellow
Start-Sleep -Seconds 20

# Show status
Write-Host "📊 Container status:" -ForegroundColor Cyan
docker-compose -f docker-compose.dev.yml ps

Write-Host ""
Write-Host "🎉 Development environment is ready!" -ForegroundColor Green
Write-Host "📊 Frontend: http://localhost:3000" -ForegroundColor Cyan
Write-Host "📊 Backend API: http://localhost:5000" -ForegroundColor Cyan
Write-Host "📊 Swagger UI: http://localhost:5000/swagger" -ForegroundColor Cyan
Write-Host ""
Write-Host "🔧 Useful commands:" -ForegroundColor Yellow
Write-Host "  View logs: docker-compose -f docker-compose.dev.yml logs -f" -ForegroundColor Gray
Write-Host "  Stop: docker-compose -f docker-compose.dev.yml down" -ForegroundColor Gray
Write-Host "  Restart: docker-compose -f docker-compose.dev.yml restart" -ForegroundColor Gray
