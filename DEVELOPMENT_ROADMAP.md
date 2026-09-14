# CodeType-Connector — Development Roadmap

## 1. Purpose

This roadmap defines the controlled implementation plan for CodeType-Connector.

Development will proceed in small, testable phases.

Core principle:

**DOCUMENT → DESIGN → IMPLEMENT → TEST → HARDEN → RELEASE**

The project must never jump directly to unrestricted computer access.

---

## 2. Current Status

Documentation foundation:

- README.md — Complete
- ARCHITECTURE.md — Complete
- PROJECT_STRUCTURE.md — Complete
- SECURITY.md — Complete
- PERMISSIONS.md — Complete
- PROTOCOL.md — Complete
- API_SPECIFICATION.md — Complete
- DEVELOPMENT_ROADMAP.md — Complete

Current phase:

**Documentation and architecture complete.**

Next phase:

**Solution and source-code foundation.**

---

## 3. Phase 1 — Documentation and Architecture

Status: **COMPLETE**

Objectives:

- Define project purpose
- Define architecture
- Define source structure
- Define security model
- Define permission model
- Define protocol
- Define API
- Define development phases

Deliverables:

- README.md
- ARCHITECTURE.md
- PROJECT_STRUCTURE.md
- SECURITY.md
- PERMISSIONS.md
- PROTOCOL.md
- API_SPECIFICATION.md
- DEVELOPMENT_ROADMAP.md

No production connector functionality is implemented in this phase.

---

## 4. Phase 2 — Solution Foundation

Status: **NEXT**

Objectives:

- Create the .NET solution
- Create source projects
- Create test projects
- Establish project references
- Establish common build configuration
- Verify the complete solution builds successfully

Planned source projects:

- CodeTypeConnector.Core
- CodeTypeConnector.Protocol
- CodeTypeConnector.Security
- CodeTypeConnector.Permissions
- CodeTypeConnector.Server
- CodeTypeConnector.Client
- CodeTypeConnector.Connectors.VisualStudio
- CodeTypeConnector.Connectors.FileSystem
- CodeTypeConnector.Connectors.ProjectManagement

Planned test projects:

- CodeTypeConnector.Core.Tests
- CodeTypeConnector.Protocol.Tests
- CodeTypeConnector.Security.Tests
- CodeTypeConnector.Permissions.Tests
- CodeTypeConnector.Server.Tests
- CodeTypeConnector.Connectors.Tests

Rule:

**The solution must build before moving to the next implementation phase.**

---

## 5. Phase 3 — Core Models and Interfaces

Objectives:

- Define request models
- Define response models
- Define resource models
- Define operation models
- Define connector interfaces
- Define session models
- Define common result and error models
- Define shared constants

Core must remain independent of Visual Studio-specific implementation.

Possible interfaces:

- IConnector
- IOperationHandler
- IResourceValidator
- ISessionManager
- IOperationExecutor

Only abstractions required by the architecture should be added.

---

## 6. Phase 4 — Protocol Implementation

Objectives:

- Implement protocol version handling
- Implement request serialization
- Implement response serialization
- Implement request ID handling
- Implement protocol validation
- Implement structured errors
- Implement version compatibility checks

Processing:

```text
Receive Request
      ↓
Validate Protocol
      ↓
Validate Request
      ↓
Process
      ↓
Return Response
