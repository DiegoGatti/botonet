# SoundBoard Project Context

## Project Goal

Build a configurable **desktop soundboard application** that can play audio into Discord voice calls.

The application should allow users to configure sounds and trigger them through a graphical interface or global keyboard shortcuts. Audio should be routed to a virtual audio device such as **VB-CABLE or VoiceMeeter**, which can then be selected as the microphone/input device in Discord.

A secondary goal of this project is **learning and practice**, especially around:

* Modern .NET development
* ASP.NET Core
* Minimal APIs
* Clean Architecture principles
* Dependency Injection
* Audio handling on Windows
* Angular
* TypeScript
* Angular Signals/RxJS
* Frontend/backend communication
* Desktop application packaging

Development will be done primarily with **VS Code and CLI tools**, rather than Visual Studio:

* `dotnet` CLI
* `ng` CLI
* `npm` / Node.js tooling
* VS Code

---

# High-Level Architecture

The application should initially keep the frontend and backend completely separated.

```text
┌─────────────────────────────┐
│          Angular            │
│                             │
│ Soundboard UI               │
│ Configuration               │
│ Categories                  │
│ Hotkeys                     │
│ Audio device selection      │
└──────────────┬──────────────┘
               │
             HTTP
               │
┌──────────────▼──────────────┐
│       ASP.NET Core          │
│        Minimal API          │
└──────────────┬──────────────┘
               │
┌──────────────▼──────────────┐
│      Application Layer      │
│                             │
│ Use cases / orchestration   │
└──────────────┬──────────────┘
               │
┌──────────────▼──────────────┐
│     Infrastructure Layer    │
│                             │
│ NAudio                      │
│ File system                 │
│ Configuration               │
│ Windows integration         │
│ Global hotkeys              │
└──────────────┬──────────────┘
               │
          Windows Audio
               │
┌──────────────▼──────────────┐
│ VB-CABLE / VoiceMeeter      │
└──────────────┬──────────────┘
               │
            Discord
```

During development:

```text
Angular
localhost:4200
       │
       │ HTTP
       ▼
ASP.NET Core
localhost:5000/5001
```

The frontend should **not initially be hosted by ASP.NET Core**.

---

# Repository Structure

The intended repository structure is approximately:

```text
soundboard/
│
├── SoundBoard.sln
│
├── src/
│   ├── SoundBoard.Api/
│   ├── SoundBoard.Application/
│   ├── SoundBoard.Domain/
│   └── SoundBoard.Infrastructure/
│
├── frontend/
│   └── soundboard-ui/
│
├── tests/
│   ├── SoundBoard.Application.Tests/
│   └── SoundBoard.Api.Tests/
│
├── docs/
│
└── sounds/
```

The initial .NET solution should remain relatively small.

Avoid introducing unnecessary enterprise patterns or libraries before they solve an actual problem.

In particular, do **not** introduce CQRS, MediatR, AutoMapper, complex Result types, repositories, Unit of Work, event buses, or other abstractions simply for architectural purity.

The architecture should evolve according to actual requirements.

---

# .NET Project Responsibilities

## SoundBoard.Domain

Contains the core domain concepts.

Potential entities/value objects include:

```text
Sound
Category
Hotkey
AudioDevice
PlaybackSession
```

The Domain project should not depend on:

* ASP.NET Core
* NAudio
* JSON serialization
* File system APIs
* Angular
* Windows-specific implementations

---

## SoundBoard.Application

Contains application use cases and abstractions.

Prefer organizing application code by **feature/use case** rather than generic folders such as `Services`, `Managers`, etc.

Example:

```text
Application/
└── Features/
    ├── Audio/
    │   ├── PlaySound/
    │   ├── StopSound/
    │   └── StopAll/
    │
    ├── Buttons/
    │   ├── CreateButton/
    │   ├── DeleteButton/
    │   └── GetButtons/
    │
    └── Devices/
        └── GetDevices/
```

Potential use cases include:

```text
PlaySound
StopSound
StopAll
ListAudioDevices

CreateButton
UpdateButton
DeleteButton
GetButtons

CreateCategory
DeleteCategory

SetHotkey

ImportConfiguration
ExportConfiguration
```

The Application layer should define interfaces for infrastructure functionality when appropriate.

For example:

```csharp
public interface IAudioPlayer
{
    // Audio abstraction
}
```

---

## SoundBoard.Infrastructure

Contains implementations that interact with external systems and the operating system.

Responsibilities may include:

```text
NAudio
Windows audio devices
File system
JSON configuration
Global Windows hotkeys
Configuration persistence
```

For example:

```text
IAudioPlayer
     ▲
     │
NAudioPlayer
```

NAudio should remain an infrastructure concern rather than leaking throughout the application.

---

## SoundBoard.Api

ASP.NET Core API responsible for exposing application functionality to Angular.

Use **Minimal APIs** initially.

Potential endpoints:

```text
GET    /api/sounds
POST   /api/sounds/{id}/play
POST   /api/sounds/{id}/stop
POST   /api/sounds/stop-all

GET    /api/devices

GET    /api/buttons
POST   /api/buttons
PUT    /api/buttons/{id}
DELETE /api/buttons/{id}
```

Endpoints should remain thin and delegate actual behavior to the Application layer.

---

# Initial Persistence

Do not introduce a database initially.

Start with JSON/filesystem-based configuration, for example:

```text
config/
├── buttons.json
├── hotkeys.json
└── settings.json
```

A possible future migration could use SQLite if persistence requirements become more complex.

---

# Angular Frontend

Angular will be the presentation layer.

The Angular application should eventually provide:

```text
Soundboard grid
Sound buttons
Categories
Search
Favorites
Audio device selection
Volume controls
Hotkey configuration
Settings
Drag & Drop
Configuration import/export
```

Potential Angular components:

```text
SoundboardComponent
SoundButtonComponent
CategoryComponent
DeviceSelectorComponent
HotkeyEditorComponent
SettingsComponent
```

The project should be useful for practicing modern Angular concepts such as:

* Standalone Components
* Signals
* RxJS
* Dependency Injection
* Reactive Forms
* Angular Router
* Angular CDK
* Drag & Drop
* HTTP communication
* Component composition

Avoid adding NgRx unless application state becomes complex enough to justify it.

---

# Development Phases

## Phase 1 — Basic .NET Backend

Start with the backend only.

Goals:

* Create the .NET solution and project structure.
* Configure project references correctly.
* Implement the initial domain model.
* Define `IAudioPlayer`.
* Create a basic audio playback use case.
* Expose it through an ASP.NET Core Minimal API.
* Initially prove that a local audio file can be played.

At the end of this phase, something similar to:

```text
POST /api/sounds/test/play
```

should cause a local sound file to play.

No Angular application is required yet.

---

## Phase 2 — NAudio and Audio Device Management

Integrate NAudio properly.

Goals:

* Play WAV/MP3 files.
* Enumerate Windows audio output devices.
* Select an output device.
* Control playback volume.
* Stop playback.
* Support `StopAll`.
* Potentially support simultaneous sounds.
* Route playback to VB-CABLE/VoiceMeeter.

The important end-to-end scenario is:

```text
SoundBoard
     ↓
NAudio
     ↓
VB-CABLE
     ↓
Discord microphone/input
```

---

## Phase 3 — Angular Frontend

Create the Angular application.

Goals:

* Build the initial soundboard grid.
* Consume the ASP.NET Core API.
* Display available sounds.
* Trigger playback.
* Stop playback.
* Select audio devices.
* Implement basic settings.
* Practice modern Angular architecture.

Keep Angular and ASP.NET Core as separate applications during development.

---

## Phase 4 — Global Hotkeys

Add configurable global keyboard shortcuts.

Example:

```text
Ctrl + 1 → Airhorn
Ctrl + 2 → Laugh
Ctrl + 3 → Applause
```

Hotkeys should work even when the SoundBoard window is not focused.

This functionality belongs primarily to the .NET/Windows side rather than Angular.

---

## Phase 5 — Configuration and UX

Expand the application into a fully configurable soundboard.

Potential features:

* Create/edit/delete sound buttons.
* Categories.
* Favorites.
* Search.
* Per-sound volume.
* Global volume.
* Drag & Drop.
* Reordering.
* Custom icons.
* Looping.
* Multiple simultaneous sounds.
* Import/export configuration.
* Persistent settings.

---

## Phase 6 — Desktop Packaging

Once the web architecture works well, package the application as a desktop application.

The final architecture should allow Angular to remain the presentation technology while .NET provides native functionality.

Possible technologies should be evaluated when reaching this phase rather than committing prematurely.

Candidates may include:

```text
Photino.NET
WebView2
Electron
Tauri
```

The goal is ultimately something like:

```text
┌───────────────────────────────┐
│       SoundBoard.exe          │
│                               │
│  Angular UI                   │
│       ↕                       │
│  .NET Backend                 │
│       ↕                       │
│  Native Windows Audio         │
└───────────────────────────────┘
```

---

# Development Principles

When assisting with this project:

1. Prefer simple implementations first.
2. Do not overengineer.
3. Keep Domain/Application independent from NAudio and ASP.NET Core where practical.
4. Keep API endpoints thin.
5. Prefer feature-oriented organization.
6. Introduce abstractions when they provide an actual boundary or testing benefit.
7. Avoid adding libraries/patterns merely because they are common in enterprise .NET projects.
8. Keep Angular and .NET independently runnable during early development.
9. Prefer CLI-based instructions compatible with VS Code.
10. Explain important architectural decisions rather than only generating code.
11. Treat the project as both a usable application and a learning exercise.
12. Prefer incremental implementation where every phase leaves the application in a runnable state.

The immediate objective is **Phase 1: establish the .NET solution and implement the smallest end-to-end backend capable of playing a local sound file through an API request.**
