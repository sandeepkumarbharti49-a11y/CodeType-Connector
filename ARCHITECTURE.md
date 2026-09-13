CodeType Connector — Architecture Specification

1. Purpose

CodeType Connector is a secure bridge between AI assistants and Visual Studio.

Its purpose is to allow AI-assisted development while keeping the developer in control of files, projects, builds, dependencies, commands, and other sensitive operations.

The connector follows:

REQUEST → PERMISSION → ACTION → RESULT

---

2. High-Level Architecture

AI Assistant
     │
     ▼
AI / MCP Protocol
     │
     ▼
CodeType Connector
     │
     ▼
Visual Studio Extension
     │
     ▼
Permission Manager
     │
     ├── File Manager
     ├── Project Manager
     ├── Build Manager
     ├── Protocol Manager
     └── Audit Logger
     │
     ▼
Visual Studio Solution

---

3. Core Components

CodeTypeConnector.VSIX

Visual Studio extension layer.

Responsibilities:

- Visual Studio integration
- Tool Window
- Connector status
- User interface
- Visual Studio lifecycle integration

---

CodeTypeConnector.Core

Core application models and shared abstractions.

Responsibilities:

- Commands
- Results
- Project information
- Permission models
- Shared interfaces
- Common validation models

---

CodeTypeConnector.Security

Security and authorization layer.

Responsibilities:

- Permission checking
- Project-scoped permissions
- Path validation
- Secret protection
- Dangerous-operation detection
- User confirmation requirements

---

CodeTypeConnector.FileSystem

Controlled file-system operations.

Supported operations:

- ReadFile
- CreateFile
- ModifyFile
- DeleteFile
- CreateDirectory
- DeleteDirectory
- MoveFile
- CopyFile
- ListDirectory
- SearchFiles

Destructive operations require additional confirmation.

---

CodeTypeConnector.ProjectSystem

Visual Studio project management.

Responsibilities:

- Detect solutions
- Detect projects
- Read project structure
- Create projects
- Add projects
- Remove projects
- Add files
- Add references
- Discover installed templates

---

CodeTypeConnector.Build

Build and test management.

Supported operations:

- Restore
- Build
- Rebuild
- Clean
- Test
- GetErrors
- GetWarnings

Build permission and Run permission are separate.

---

CodeTypeConnector.Protocol

Communication protocol between AI and the connector.

Responsibilities:

- Command validation
- Request processing
- Result formatting
- Error handling
- Protocol versioning

---

CodeTypeConnector.MCP

Model Context Protocol integration layer.

Responsibilities:

- MCP server integration
- Tool definitions
- Resource definitions
- Request/response mapping
- AI tool authorization

MCP integration will be expanded after the V1 foundation is stable.

---

CodeTypeConnector.Logging

Audit and diagnostic logging.

Every important operation should record:

- Timestamp
- Project
- Command
- Path
- Permission
- User decision
- Result
- Error information

Secrets must never be written to logs.

---

4. Permission Model

CodeType Connector uses a default-deny model.

Permission Levels

Level| Name| Description
0| Denied| Operation blocked
1| Read| Read-only access
2| Write| Create/modify files
3| Project| Project management
4| Build| Build/test operations
5| Advanced| High-risk operations

---

5. Permission Types

READ_FILES
CREATE_FILES
MODIFY_FILES
DELETE_FILES

CREATE_PROJECT
MODIFY_PROJECT
DELETE_PROJECT

BUILD
TEST

PACKAGE_INSTALL
PACKAGE_REMOVE

NETWORK_ACCESS
SHELL_COMMAND
RUN_APPLICATION
DEBUG

GIT_READ
GIT_WRITE

---

6. Project-Scoped Security

Permissions should normally be assigned to a specific project.

Example:

PHOTVIA
├── READ_FILES      ALLOWED
├── CREATE_FILES    ALLOWED
├── MODIFY_FILES    ALLOWED
├── BUILD           ALLOWED
├── DELETE_FILES    DENIED
└── RUN_APPLICATION DENIED

A permission granted to one project must not automatically grant access to another project.

---

7. File-System Security

The connector must never provide unrestricted access to the entire computer.

The user selects approved workspace/project roots.

Example:

Allowed:
C:\Projects\MyApplication\

Denied:
C:\Windows\System32\

Path traversal attacks such as:

..\..\Windows\System32

must be detected and blocked.

---

8. Secret Protection

The connector must protect sensitive information.

Examples:

- Passwords
- API keys
- JWT secrets
- SMTP credentials
- Google App Passwords
- Payment credentials
- Private keys
- Access tokens
- Database passwords

Secrets must not automatically be sent to the AI.

Secrets must not appear in audit logs.

---

9. Request Processing Pipeline

Every AI command follows:

AI Request
    │
    ▼
Parse Command
    │
    ▼
Validate Command
    │
    ▼
Validate Project
    │
    ▼
Validate Path
    │
    ▼
Check Permission
    │
    ▼
User Confirmation
    │
    ▼
Execute Action
    │
    ▼
Validate Result
    │
    ▼
Audit Log
    │
    ▼
Return Result

---

10. Example Command

{
  "command": "create_file",
  "project": "ExampleProject",
  "path": "MainActivity.kt",
  "content": "...",
  "reason": "Create application entry screen"
}

Before execution:

Command validation
        ↓
Path validation
        ↓
Permission validation
        ↓
User approval
        ↓
Execution

---

11. Example Result

{
  "success": true,
  "command": "create_file",
  "project": "ExampleProject",
  "path": "MainActivity.kt"
}

Error example:

{
  "success": false,
  "error": "PermissionDenied",
  "message": "The requested operation requires user approval."
}

---

12. Diff Preview

Important file modifications should support a before/after preview.

Before
----------------
old code

After
----------------
new code

The user can:

APPLY
REJECT

The AI must not silently overwrite important existing code.

---

13. Audit Log

Example:

Timestamp:
Project:
Command:
Path:
Permission:
User Decision:
Result:
Error:

Sensitive information must be filtered before logging.

---

14. Emergency Stop

The connector must provide an emergency STOP mechanism.

When activated:

- Pending actions are cancelled.
- New connector actions are blocked.
- Automated workflows stop.
- User can manually reconnect.

---

15. Existing Project Protection

CodeType Connector must understand the existing project before making changes.

It must avoid:

- Unrequested refactoring
- Unrequested dependency changes
- Unrequested file deletion
- Unrequested project changes
- Unrequested configuration changes

Only the requested scope should be modified.

---

16. V1 Scope

V1 focuses on:

VSIX Foundation
Tool Window
Connector Status
Permission System
Read File
Create File
Modify File
Build
Get Errors
Audit Log

Delete and Run operations remain disabled during the initial MVP.

---

17. V2 Scope

V2 will add:

Project Creation
Template Discovery
Package Management
Testing
Diff Preview
Multi-project Support
Advanced Permissions

---

18. V3 Scope

V3 will add:

MCP Integration
Debugging
AI Build/Fix Cycle
Project Scaffolding
Workspace Automation
Controlled AI Automation

---

19. Development Rules

1. Default deny.
2. User controls permissions.
3. Permissions are project-scoped.
4. No unrestricted PC access.
5. No automatic destructive operations.
6. Important modifications should support diff preview.
7. Secrets must be protected.
8. Important actions must be audited.
9. Emergency STOP must be available.
10. Never modify unrelated projects.
11. Never bypass Visual Studio security.
12. Never bypass operating-system security.
13. Preserve existing working code unless explicitly requested.

---

20. Future Vision

The long-term goal is to allow a developer to give an AI a development request such as:

Create an Android application.

CodeType Connector could then:

Understand Request
       ↓
Detect Technology
       ↓
Discover Available Templates
       ↓
Request Permissions
       ↓
Create Project
       ↓
Generate Code
       ↓
Add Approved Dependencies
       ↓
Build
       ↓
Read Errors
       ↓
Propose Fix
       ↓
Request Approval
       ↓
Apply Fix
       ↓
Build Again
       ↓
Return Result

The developer remains the final authority throughout the process.

---

21. Design Principle

CodeType Connector should make AI powerful without making the AI uncontrolled.

The user owns the computer.

The user owns the projects.

The user owns the permissions.

The AI assists.
