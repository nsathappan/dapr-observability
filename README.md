# dapr-observability

# SampleApplication

This repository contains a .NET 9 solution with multiple services, each running in its own Docker container. The solution includes Dapr sidecars for each service to enable distributed application patterns.

## Services

### App1Service
- **Description**: This is the first application service.
- **Dockerfile**: `App1/Dockerfile`
- **Ports**: 40000 (external) -> 41000 (internal)

### App2Service
- **Description**: This is the second application service.
- **Dockerfile**: `App2/Dockerfile`
- **Ports**: 40001 (external) -> 41001 (internal)

## Dapr Sidecars

Each service has an associated Dapr sidecar to handle service-to-service communication, state management, and other Dapr features.

### App1Service-sidecar
- **Image**: `daprio/daprd:latest`
- **Command**:
- **Volumes**: `./components/:/components`
- **Network Mode**: `service:app1Service`

### App2Service-sidecar
- **Image**: `daprio/daprd:latest`
- **Command**:
- **Volumes**: `./components/:/components`
- **Network Mode**: `service:app2Service`

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/get-started)
- [Dapr CLI](https://docs.dapr.io/getting-started/install-dapr-cli/)

## Getting Started

1. **Clone the repository**:
   git clone https://github.com/your-repo/sampleapplication.git cd sampleapplication

2. **Build and run the services**:
   docker-compose up --build

3. **Access the services**:
   - App1Service: `http://localhost:40000`
   - App2Service: `http://localhost:40001`

## Project Structure

. ├── App1 │   ├── Dockerfile │   ├── WeatherForecast.cs │   └── ... ├── App2 │   ├── Dockerfile │   └── ... ├── components │   ├── config.yaml │   └── ... ├── docker-compose.yml └── .gitignore
