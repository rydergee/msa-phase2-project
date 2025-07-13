#!/bin/bash

# MockMate Development Setup Script

set -e

echo "🛠️ Setting up MockMate Development Environment..."

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "❌ Docker is not running. Please start Docker and try again."
    exit 1
fi

# Stop any existing containers
echo "🛑 Stopping any existing containers..."
docker-compose -f docker-compose.dev.yml down

# Build and start development containers
echo "🔨 Building and starting development containers..."
docker-compose -f docker-compose.dev.yml up --build -d

# Wait for services to be ready
echo "⏳ Waiting for services to be ready..."
sleep 20

# Run database migrations if needed
echo "📊 Setting up database..."
docker-compose -f docker-compose.dev.yml exec backend dotnet ef database update || echo "⚠️ No migrations to run"

# Show status
echo "📊 Container status:"
docker-compose -f docker-compose.dev.yml ps

echo "🎉 Development environment is ready!"
echo "📊 Frontend: http://localhost:3000"
echo "📊 Backend API: http://localhost:5000"
echo "📊 Swagger UI: http://localhost:5000/swagger"
echo ""
echo "🔧 Useful commands:"
echo "  View logs: docker-compose -f docker-compose.dev.yml logs -f"
echo "  Stop: docker-compose -f docker-compose.dev.yml down"
echo "  Restart: docker-compose -f docker-compose.dev.yml restart"
