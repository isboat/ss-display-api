# OnScreenSync Display API

The Display API is the backend used by registered TVs. It lets a TV pair with an OnScreenSync organisation, identify itself on later visits, and fetch the signage assigned to it. This gives TV players a device-specific interface to content managed through the dashboard.

## What it supports

- Issues short-lived activation codes and lets the TV check whether an operator has approved a code. After approval, the device receives credentials and can refresh them.
- Returns the device's identity and assigned screen content. The response carries layout and content data for the TV to render, including referenced media, menus, and playlists where configured.
- Negotiates real-time connectivity and registers devices for targeted messages so players can react to updates. Its health endpoint checks the database and messaging service.

## How it is organised

The [Display solution](../src/Display) contains Display.Api for HTTP endpoints and hosting, Display.Services for pairing, tokens, content, and messaging, Display.Repositories for database access, and Display.Models and Display.Shared for common data and building blocks. [The README](../README.md) lists the main configuration and startup requirements.

## What it relies on

- **MongoDB** stores device registrations, device and screen records, and other data needed to resolve assigned content.
- **Azure SignalR Service** provides real-time communication with displays. The **Management Dashboard API and UI** provide the operator side of pairing, assignment, and publishing.
- **Android TV, Tizen, and webOS players** consume this API. They discover its routes through bootstrap configuration published by the OnScreenSync website.
- **ASP.NET Core on .NET 8** hosts the API. JWT bearer authentication identifies registered devices, and Swagger supports API discovery during development.
