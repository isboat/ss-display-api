# ss-display-api

`ss-display-api` is a .NET 8 API for display devices. It supports device-code registration, JWT-based device authorization, MongoDB-backed device/content lookup, and Azure SignalR notifications.

## Solution layout

- `src/Display/Display.Api` - ASP.NET Core API, controllers, middleware, and SignalR hosted service.
- `src/Display/Display.Models` - request/response models and configuration models.
- `src/Display/Display.Services` - device auth, JWT, content, device, and SignalR business logic.
- `src/Display/Display.Repositories` - MongoDB repository implementations.
- `src/Display/Display.Shared` - shared constants, exceptions, and time provider abstractions.

## Required configuration

Configure these sections through `appsettings`, environment variables, user secrets, or your deployment secret store:

```json
{
  "Authentication": {
    "Disabled": false
  },
  "Jwt": {
    "Issuer": "https://issuer.example",
    "Audience": "display-api",
    "SigningKey": "replace-with-a-long-secret-from-your-secret-store"
  },
  "DeviceAuthentication": {
    "VerificationUrl": "https://dashboard.onscreensync.com/#/device/auth",
    "DeviceCodeExpiresInSeconds": 1800,
    "PollingIntervalSeconds": 5,
    "AccessTokenLifetimeDays": 3,
    "RefreshTokenLifetimeDays": 365
  },
  "SignalR": {
    "AzureSignalRConnectionString": "Endpoint=...;AccessKey=...;Version=1.0;"
  },
  "MongoSettings": {
    "ConnectionString": "mongodb://localhost:27017"
  }
}
```

> Do not commit production JWT signing keys, MongoDB credentials, or Azure SignalR keys.

## Run locally

```bash
dotnet restore src/Display/Display.sln
dotnet run --project src/Display/Display.Api/Display.Api.csproj
```

Swagger is enabled when the ASP.NET Core environment is `Development`.

## Build

```bash
dotnet build src/Display/Display.sln
```

## Docker

The API Dockerfile uses .NET 8 SDK/runtime images to match the project target framework.

```bash
docker build -f src/Display/Display.Api/Dockerfile src/Display -t ss-display-api
```
