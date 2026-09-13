CodeType Connector

Secure AI Connector for Visual Studio

CodeType Connector is a security-first bridge designed to connect AI assistants with a user's Visual Studio development environment.

It allows AI systems to work with Visual Studio projects through a controlled, permission-based workflow while keeping the user in control of sensitive operations.

Core Principle

REQUEST → PERMISSION → ACTION → RESULT

Every sensitive operation must pass through the permission system before execution.

Goals

CodeType Connector is designed to allow an AI assistant to:

- Read project and solution structure
- Read source files
- Create files and folders
- Modify existing files
- Create and manage projects
- Discover installed Visual Studio templates
- Add project references
- Manage approved dependencies
- Build projects
- Run tests
- Read build errors and warnings
- Preview important changes
- Eventually integrate debugging workflows
- Communicate through an AI/MCP protocol

Security Model

CodeType Connector follows a DEFAULT DENY security model.

The AI does not automatically receive unrestricted access to the user's computer.

Permission Levels

Level| Description
0| Denied
1| Read
2| Write
3| Project
4| Build
5| Advanced

Permissions should be scoped to individual projects whenever possible.

Permission Types

Examples include:

- "READ_FILES"
- "CREATE_FILES"
- "MODIFY_FILES"
- "DELETE_FILES"
- "CREATE_PROJECT"
- "MODIFY_PROJECT"
- "DELETE_PROJECT"
- "BUILD"
- "TEST"
- "PACKAGE_INSTALL"
- "PACKAGE_REMOVE"
- "NETWORK_ACCESS"
- "SHELL_COMMAND"
- "RUN_APPLICATION"
- "DEBUG"
- "GIT_READ"
- "GIT_WRITE"

High-risk operations require explicit user confirmation.

Architecture

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

Security Rules

CodeType Connector must:

1. Use default-deny permissions.
2. Never provide unrestricted computer access.
3. Use project-scoped permissions.
4. Require approval for sensitive operations.
5. Protect secrets and credentials.
6. Preview important file modifications.
7. Maintain an audit log.
8. Provide an emergency stop / kill switch.
9. Never modify unrelated projects.
10. Never perform destructive operations automatically.
11. Never bypass Visual Studio or operating-system security.
12. Preserve existing working code unless the user explicitly requests a change.

File System Protection

Access should be restricted to user-approved project/workspace roots.

Protected system locations such as:

C:\Windows\System32

must not be accessible through unrestricted AI commands.

Potentially destructive operations such as delete, move, overwrite, or bulk modification require additional confirmation.

Secret Protection

The connector must detect and protect sensitive information such as:

- Passwords
- API keys
- JWT secrets
- SMTP passwords
- Google App Passwords
- Payment credentials
- Private keys
- Connection secrets
- Access tokens

Secrets must not be automatically exposed to the AI or written to audit logs.

V1 MVP

The first version will focus on:

- Visual Studio extension foundation
- Connector status
- Tool Window
- Permission UI
- Read file
- Create file
- Modify file
- Build
- Get build errors
- Audit log
- Default-deny security

Delete and Run operations will remain disabled initially.

V2

Planned V2 capabilities:

- Project creation
- Visual Studio template discovery
- Package management
- Testing
- Diff preview
- Multi-project support
- Advanced permissions
- Improved project management

V3

Planned V3 capabilities:

- MCP integration
- AI-assisted build/fix workflow
- Debugging integration
- Automated project scaffolding
- Workspace management
- Controlled AI automation

Example Command

{
  "command": "create_file",
  "project": "AI-Video-Enhancer",
  "path": "MainActivity.kt",
  "content": "...",
  "reason": "Create application entry screen"
}

The connector must validate:

Command
   ↓
Path
   ↓
Project Scope
   ↓
Permission
   ↓
User Confirmation
   ↓
Action
   ↓
Result
   ↓
Audit Log

Example Result

{
  "success": true,
  "command": "create_file",
  "project": "AI-Video-Enhancer",
  "path": "MainActivity.kt"
}

Protected Project Principle

Existing production projects must be protected from unrelated modifications.

For example, when working on one feature, the connector must not automatically modify unrelated systems, dependencies, or working code.

The user must explicitly authorize changes outside the requested scope.

Emergency Stop

CodeType Connector will provide an emergency stop mechanism that immediately prevents pending or future connector actions.

Roadmap

V1
 │
 ├── VSIX Foundation
 ├── Tool Window
 ├── Permission System
 ├── File Access
 ├── Build
 └── Audit Log
       │
       ▼
V2
 │
 ├── Project Creation
 ├── Templates
 ├── Dependencies
 ├── Testing
 ├── Diff Preview
 └── Multi-project
       │
       ▼
V3
 │
 ├── MCP
 ├── Debugging
 ├── AI Build/Fix
 ├── Scaffolding
 └── Workspace Automation

Future Vision

A user could eventually tell the AI:

«"Create an Android application for me."»

CodeType Connector could then:

1. Understand the requested technology.
2. Detect installed Visual Studio capabilities where applicable.
3. Discover available templates.
4. Create the approved project.
5. Generate the required files.
6. Request permission for dependencies.
7. Build the project.
8. Read build errors.
9. Propose fixes.
10. Request approval where required.
11. Apply approved fixes.
12. Build again.
13. Report the final result.

Development Philosophy

CodeType Connector is built around:

Security + Permission + Transparency + User Control

The AI should be powerful enough to assist with real development while the user remains the final authority over their computer, projects, files, and permissions.

License

License will be defined as the project matures.
