# MockMate Deployment Guide

This guide covers deploying MockMate in various environments, from local development to production.

## 🚀 Quick Start

### Prerequisites

- Docker & Docker Compose
- Git
- .NET 8.0 SDK (for local development)
- Node.js 18+ (for local development)

### Development Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/msa-phase2-project.git
   cd msa-phase2-project
   ```

2. **Run development environment**
   ```bash
   # Linux/Mac
   chmod +x scripts/setup-dev.sh
   ./scripts/setup-dev.sh
   
   # Windows
   .\scripts\setup-dev.ps1
   ```

3. **Access the application**
   - Frontend: http://localhost:3000
   - Backend API: http://localhost:5000
   - Swagger UI: http://localhost:5000/swagger

## 🐳 Docker Deployment

### Production Deployment

1. **Prepare environment variables**
   ```bash
   cp .env.example .env
   # Edit .env with your production values
   ```

2. **Deploy with Docker Compose**
   ```bash
   chmod +x scripts/deploy-production.sh
   ./scripts/deploy-production.sh
   ```

### Development with Docker

```bash
# Start development environment
docker-compose -f docker-compose.dev.yml up --build

# View logs
docker-compose -f docker-compose.dev.yml logs -f

# Stop environment
docker-compose -f docker-compose.dev.yml down
```

## ☁️ Cloud Deployment

### Azure App Service

1. **Backend Deployment**
   ```bash
   # Build and publish
   dotnet publish backend/MockMate.Api.csproj -c Release -o publish
   
   # Deploy to Azure App Service
   az webapp deployment source config-zip \
     --resource-group your-resource-group \
     --name your-app-name \
     --src publish.zip
   ```

2. **Frontend Deployment**
   ```bash
   # Build frontend
   cd frontend
   npm run build
   
   # Deploy to Azure Static Web Apps or App Service
   az staticwebapp deploy \
     --name your-static-app \
     --source-location dist
   ```

### AWS Deployment

1. **Using Elastic Beanstalk**
   ```bash
   # Install EB CLI
   pip install awsebcli
   
   # Initialize and deploy
   eb init
   eb create mockmate-production
   eb deploy
   ```

2. **Using ECS with Fargate**
   ```bash
   # Build and push Docker images
   docker build -t mockmate-api:latest -f backend/Dockerfile .
   docker build -t mockmate-frontend:latest frontend/
   
   # Tag and push to ECR
   docker tag mockmate-api:latest your-ecr-repo:latest
   docker push your-ecr-repo:latest
   ```

### Google Cloud Platform

```bash
# Deploy using Cloud Run
gcloud run deploy mockmate-api \
  --image gcr.io/your-project/mockmate-api \
  --platform managed \
  --region us-central1 \
  --allow-unauthenticated
```

## 🔧 Configuration

### Environment Variables

| Variable | Description | Required | Default |
|----------|-------------|----------|---------|
| `JWT_SECRET_KEY` | Secret key for JWT token generation | Yes | - |
| `CONNECTION_STRING` | Database connection string | No | SQLite |
| `CORS_ORIGINS` | Allowed CORS origins | No | localhost:3000 |
| `ASPNETCORE_ENVIRONMENT` | ASP.NET Core environment | No | Production |
| `LOG_LEVEL` | Logging level | No | Information |

### Database Configuration

**SQLite (Default)**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=mockmate.db"
  }
}
```

**SQL Server**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=server;Database=MockMate;Trusted_Connection=true;"
  }
}
```

**PostgreSQL**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=mockmate;Username=user;Password=password"
  }
}
```

## 🔒 Security Considerations

### Production Checklist

- [ ] Set strong JWT secret key (minimum 32 characters)
- [ ] Configure HTTPS/TLS certificates
- [ ] Set up proper CORS origins
- [ ] Enable rate limiting
- [ ] Configure security headers
- [ ] Set up monitoring and logging
- [ ] Regular security updates
- [ ] Database backups
- [ ] Environment variable encryption

### SSL/TLS Setup

1. **Using Let's Encrypt with Nginx**
   ```bash
   # Install certbot
   sudo apt-get install certbot python3-certbot-nginx
   
   # Obtain certificate
   sudo certbot --nginx -d yourdomain.com
   ```

2. **Using custom certificates**
   ```yaml
   # docker-compose.yml
   nginx:
     volumes:
       - ./ssl:/etc/nginx/ssl:ro
   ```

## 📊 Monitoring & Logging

### Health Checks

The application includes built-in health checks:
- Backend: `GET /health`
- Database connectivity check
- Memory usage monitoring

### Logging

Configure structured logging in production:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```

### Application Insights (Azure)

```json
{
  "ApplicationInsights": {
    "InstrumentationKey": "your-key"
  }
}
```

## 🔄 CI/CD Pipeline

The project includes GitHub Actions workflows:

- **Pull Request**: Runs tests and builds
- **Main Branch**: Deploys to staging and production
- **Manual**: Allows manual deployment triggers

### Setting up GitHub Actions

1. **Configure secrets**
   - `JWT_SECRET_KEY`
   - `AZURE_CREDENTIALS` (if using Azure)
   - `AWS_ACCESS_KEY_ID` (if using AWS)
   - `GCP_SERVICE_ACCOUNT_KEY` (if using GCP)

2. **Update workflow files**
   - Modify `.github/workflows/ci-cd.yml`
   - Add deployment-specific steps

## 🚨 Troubleshooting

### Common Issues

1. **Port conflicts**
   ```bash
   # Check ports in use
   netstat -tulpn | grep :5000
   
   # Kill process using port
   sudo kill -9 $(lsof -t -i:5000)
   ```

2. **Docker build failures**
   ```bash
   # Clean Docker cache
   docker system prune -a
   
   # Rebuild without cache
   docker-compose build --no-cache
   ```

3. **Database connection issues**
   ```bash
   # Check container logs
   docker-compose logs backend
   
   # Reset database
   docker-compose down -v
   docker-compose up -d
   ```

### Performance Optimization

1. **Database optimization**
   - Add database indexes
   - Enable query caching
   - Optimize connection pooling

2. **Frontend optimization**
   - Enable gzip compression
   - Set up CDN
   - Optimize bundle size

3. **API optimization**
   - Implement response caching
   - Add rate limiting
   - Optimize database queries

## 📞 Support

For deployment issues:
1. Check the logs: `docker-compose logs`
2. Verify environment variables
3. Check network connectivity
4. Review security settings
5. Consult cloud provider documentation

## 📝 Changelog

Track deployment versions and changes in [CHANGELOG.md](./CHANGELOG.md).
