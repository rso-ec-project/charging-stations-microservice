# charging-stations-microservice

This microservice is part of the Charging Stations Application and provides endpoints with information about Tenants, Charging Stations, Chargers and Charger Models.

# Prerequisites

- [Visual Studio](https://visualstudio.microsoft.com/vs/)
- [.NET 5.0](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)

# Setup

Open the .sln file with Visual Studio and restore NuGet packages:

    Right click Solution -> Restore NuGet Packages

Open Package Manager Console and execute these commands to create the database and it's tables:

    $env:DB_HOST = "{Host}";
    $env:DB_NAME = "{Database}";
    $env:DB_USERNAME = "{Username}";
    $env:DB_PASSWORD= "{Password}";
    Update-Database;

When running locally with Docker, move Dockerfile to the ChargingStations.API project and add Environment Variables in one line as follows:

    ENV DB_HOST={Host} DB_NAME={Database} DB_USERNAME={Username} DB_PASSWORD={Password} HOST_IP=host.docker.internal


# Details

## Frameworks & Tools

This service is developed in the .NET 5.0 Framework, using Visual Studio 2019 as the IDE of choice. Entity Framework Core is used as an ORM and to generate the database from defined entity classes.

## Continuous Integration

A GitHub Actions CI pipeline is set up for this service, which builds a Docker Image from the project and pushes it to DockerHub: https://hub.docker.com/repository/docker/anzx10/charging-stations-microservice.

## Deployment

This service is deployed to the Kubernetes Cluster using **Deployments**, defined in the **/k8s/deployment.yaml** file, accompanied by a Load Balancer deployment.

## Database

This service is using a PostgreSQL Database, hosted on ElephantSQL. It consists of 4 tables for the following entities:
- **Tenant**: Users that provide Charging Stations to consumers. Can have multiple Charging Stations.
- **ChargingStation**: Stations owned by a specific Tenant. Can have multiple Chargers.
- **Charger**: An actual Charger in a Charging Station, which consumers can book to charge their vehicles. Can only have one ChargerModel.
- **ChargerModel**: Basic information about the model of the Charger.

## Configuration

This microservice is configured using Environment Variables. Currently, only configuration for the database is required. The HOST_IP variable is used to find the Consul server either locally or in a cluster. The password is safely stored in Kubernetes Secrets, while other information is provided during the deployment process on the Cluster.

## Endpoints

- GET /Tenants
- GET /Tenants/{id}
- GET /Tenants/{id}/ChargingStations
- GET /ChargingStations
- GET /ChargingStations/{id}
- GET /ChargingStations/{id}/Chargers
- GET /Chargers/{id}
- GET /ChargerModels

All endpoints are described with the OpenAPI 3 standard on the **/swagger/index.html** endpoint of the service. The API is also versioned, which adds a version segment to the path of the endpoints (e.g. api/**v1**/ChargingStations).

## HealthChecks

Two Health Checks are implemented in this service. One is a basic **Liveness** check and the second one is a **Readiness** check on the database.
