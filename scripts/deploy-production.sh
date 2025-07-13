#!/bin/bash

# MockMate Production Deployment Script

set -e

echo "🚀 Starting MockMate Production Deployment..."

# Check if required environment variables are set
if [ -z "$JWT_SECRET_KEY" ]; then
    echo "❌ JWT_SECRET_KEY environment variable is required"
    exit 1
fi

# Pull latest images
echo "📦 Pulling latest Docker images..."
docker-compose pull

# Stop existing containers
echo "🛑 Stopping existing containers..."
docker-compose down

# Start new containers
echo "🚀 Starting new containers..."
docker-compose up -d

# Wait for services to be ready
echo "⏳ Waiting for services to be ready..."
sleep 30

# Run health checks
echo "🏥 Running health checks..."
if curl -f http://localhost:5000/health; then
    echo "✅ Backend health check passed"
else
    echo "❌ Backend health check failed"
    exit 1
fi

if curl -f http://localhost:3000; then
    echo "✅ Frontend health check passed"
else
    echo "❌ Frontend health check failed"
    exit 1
fi

# Clean up old images
echo "🧹 Cleaning up old images..."
docker image prune -f

echo "🎉 Deployment completed successfully!"
echo "📊 Access your application at: http://localhost:3000"
echo "📊 API documentation at: http://localhost:5000/swagger"
