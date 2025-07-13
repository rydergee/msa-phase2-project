# Frontend Dockerfile
FROM node:18-alpine AS build

WORKDIR /app

# Copy package files
COPY package*.json ./
RUN npm ci --only=production

# Copy source code
COPY . .

# Build the application
RUN npm run build

# Production stage
FROM nginx:alpine AS production

# Copy built assets from build stage
COPY --from=build /app/dist /usr/share/nginx/html

# Copy nginx configuration
COPY nginx.conf /etc/nginx/nginx.conf

# Create a non-root user
RUN addgroup -g 1001 -S mockmate && \
    adduser -S mockmate -u 1001

# Change ownership of nginx directories
RUN chown -R mockmate:mockmate /var/cache/nginx && \
    chown -R mockmate:mockmate /var/log/nginx && \
    chown -R mockmate:mockmate /etc/nginx/conf.d

# Switch to non-root user
USER mockmate

# Expose port
EXPOSE 3000

CMD ["nginx", "-g", "daemon off;"]
