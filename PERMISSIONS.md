CodeType-Connector — Permission System

1. Overview

CodeType-Connector uses a permission-based security model to control what an AI system or connected application can do on the user's computer.

The connector must never provide unrestricted access by default.

Every protected operation must have an appropriate permission.

The core principle is:

«No permission = No operation.»

---

2. Permission Goals

The permission system is designed to provide:

- Least-privilege access
- Explicit authorization
- User-controlled access
- Resource-level restrictions
- Project-level restrictions
- Operation-level restrictions
- Connector isolation
- Permission expiration
- Permission revocation
- Auditability
- Secure defaults

---

3. Permission Hierarchy

Permissions are organized into multiple levels.

Permission System
│
├── Connector
│
├── Resource
│
├── Project
│
├── File
│
└── Operation

A permission should identify both what can be accessed and what operation can be performed.

Example:

Project: MyApplication
Resource: Source Files
Operation: READ

---

4. Permission Categories

The initial permission categories are:

PROJECT
FILE
BUILD
EXECUTION
IDE
SYSTEM
GIT
NETWORK

Additional categories may be added in future versions.

---

5. File Permissions

5.1 READ_FILE

Allows reading an authorized file.

Example:

READ_FILE
Project: MyApplication
Path: app/src/MainActivity.kt

The permission does not allow modification or deletion.

---

5.2 CREATE_FILE

Allows creating a new file inside an authorized location.

---

5.3 WRITE_FILE

Allows modifying an existing file.

This permission must not automatically include:

- DELETE_FILE
- RENAME_FILE
- MOVE_FILE

---

5.4 DELETE_FILE

Allows deleting authorized files.

This is a sensitive permission and should normally require explicit user approval.

---

5.5 RENAME_FILE

Allows renaming an authorized file.

---

5.6 MOVE_FILE

Allows moving an authorized file.

The destination must also be inside an authorized resource scope.

---

6. Project Permissions

6.1 READ_PROJECT

Allows reading project information.

Examples:

- Project name
- Project type
- Project structure
- Project files
- Project configuration

---

6.2 CREATE_PROJECT

Allows creating a new development project.

This is a sensitive operation.

User approval should normally be required.

---

6.3 MODIFY_PROJECT

Allows modifying project configuration or project structure.

Examples:

- Add project files
- Remove project files
- Modify project settings
- Modify project configuration

---

6.4 DELETE_PROJECT

Allows deleting a project.

This is a high-risk permission.

It should require explicit user approval.

---

7. Build Permissions

7.1 BUILD_PROJECT

Allows building an authorized project.

A build may execute tools, compilers, scripts, or project-defined build actions.

Therefore this is a privileged permission.

---

7.2 REBUILD_PROJECT

Allows rebuilding an authorized project.

---

7.3 CLEAN_PROJECT

Allows cleaning project build artifacts.

---

7.4 RESTORE_PROJECT

Allows restoring project dependencies.

Dependency restoration may download external packages and should therefore be treated as a controlled operation.

---

8. Execution Permissions

8.1 RUN_PROJECT

Allows running an authorized application.

Because application execution may execute arbitrary project code, this permission is sensitive.

---

8.2 EXECUTE_COMMAND

Allows executing a system or development command.

This is one of the highest-risk permissions in CodeType-Connector.

It must not be enabled by default.

Where possible, command execution should use an allowlist and additional restrictions.

---

8.3 EXECUTE_SCRIPT

Allows execution of scripts.

Examples:

PowerShell
Batch
Shell
Python
Node.js
Other supported scripts

Script execution requires explicit authorization.

---

9. Visual Studio Permissions

The Visual Studio connector may expose specialized permissions.

Examples:

VISUAL_STUDIO_READ
VISUAL_STUDIO_OPEN_PROJECT
VISUAL_STUDIO_CREATE_PROJECT
VISUAL_STUDIO_MODIFY_PROJECT
VISUAL_STUDIO_BUILD
VISUAL_STUDIO_RUN
VISUAL_STUDIO_DEBUG

Each permission should be independently controlled.

For example:

VISUAL_STUDIO_READ

must not automatically grant:

VISUAL_STUDIO_BUILD

---

10. IDE Permissions

Future IDE connectors may use permissions such as:

IDE_READ
IDE_OPEN_PROJECT
IDE_MODIFY_PROJECT
IDE_BUILD
IDE_RUN
IDE_DEBUG

The same permission model should apply to:

- Visual Studio
- Visual Studio Code
- JetBrains IDEs
- Android Studio
- Other supported IDEs

---

11. Git Permissions

Future Git integration may support:

GIT_READ
GIT_STATUS
GIT_DIFF
GIT_STAGE
GIT_COMMIT
GIT_BRANCH
GIT_PULL
GIT_PUSH

High-risk operations such as push should require explicit authorization.

---

12. Network Permissions

Network access must be separately controlled.

Possible permissions:

NETWORK_READ
NETWORK_CONNECT
NETWORK_DOWNLOAD
NETWORK_UPLOAD

A connector must not automatically receive unrestricted network access.

---

13. System Permissions

System-level operations are highly privileged.

Examples:

SYSTEM_READ
SYSTEM_CONFIGURATION
PROCESS_READ
PROCESS_START
PROCESS_STOP
SERVICE_CONTROL

System permissions should be disabled by default.

---

14. Permission Scope

A permission must have a defined scope.

Possible scopes:

GLOBAL
WORKSPACE
PROJECT
DIRECTORY
FILE
OPERATION

Example:

Permission:
WRITE_FILE

Scope:
PROJECT

Resource:
MyApplication

This means the client can modify files only inside the authorized project.

---

15. Permission Example

A safe permission grant may look like:

Client:
AI-Client

Connector:
VisualStudio

Project:
MyApplication

Permissions:
READ_PROJECT
READ_FILE
WRITE_FILE
BUILD_PROJECT

Scope:
MyApplication

Expiration:
Session

The client cannot automatically access another project.

---

16. Permission Bundles

For convenience, permissions may be grouped into predefined bundles.

Read Only

READ_PROJECT
READ_FILE
VISUAL_STUDIO_READ
GIT_READ
GIT_STATUS
GIT_DIFF

Developer

READ_PROJECT
READ_FILE
CREATE_FILE
WRITE_FILE
RENAME_FILE
MOVE_FILE
BUILD_PROJECT
RUN_PROJECT

Administrator

Administrator permissions may include additional privileged operations.

Administrator access must still follow authentication and security validation.

Bundles must not bypass individual security checks.

---

17. Default Permission

The default permission set for a new client must be:

NONE

No protected operation should be available until permission is granted.

---

18. Permission Request

When a client requires a permission that it does not currently have, it should request that permission.

Example:

Permission Request

Client:
AI-Client

Requested Permission:
WRITE_FILE

Project:
MyApplication

File:
MainActivity.kt

Reason:
Modify the application code

User Approval:
Required

The user may:

ALLOW
DENY

---

19. Temporary Permissions

Permissions may be temporary.

Possible expiration policies:

ONE_OPERATION
SESSION
TIME_LIMITED
UNTIL_REVOKED

Example:

WRITE_FILE
Expiration:
ONE_OPERATION

After the operation completes, the permission expires.

---

20. Permission Revocation

The user must be able to revoke permissions.

Revocation should take effect immediately or as soon as safely possible.

Example:

Client
  │
  ├── READ_FILE     ALLOWED
  ├── WRITE_FILE    ALLOWED
  └── BUILD_PROJECT REVOKED

After revocation:

BUILD_PROJECT → DENIED

---

21. Permission Escalation

A client must never be able to grant additional permissions to itself.

Example:

Current Permission:
READ_FILE

Requested Operation:
WRITE_FILE

Result:
PERMISSION DENIED

The client must request authorization through the permission system.

---

22. Sensitive Permissions

The following permissions should normally require explicit user confirmation:

WRITE_FILE
DELETE_FILE
CREATE_PROJECT
MODIFY_PROJECT
DELETE_PROJECT
BUILD_PROJECT
RUN_PROJECT
EXECUTE_COMMAND
EXECUTE_SCRIPT
GIT_COMMIT
GIT_PUSH
NETWORK_UPLOAD
SYSTEM_CONFIGURATION
PROCESS_START
PROCESS_STOP

Additional permissions may be classified as sensitive in future versions.

---

23. High-Risk Permissions

The following permissions are considered high-risk:

EXECUTE_COMMAND
EXECUTE_SCRIPT
SYSTEM_CONFIGURATION
SERVICE_CONTROL
PROCESS_START
PROCESS_STOP
DELETE_PROJECT
GIT_PUSH
NETWORK_UPLOAD

High-risk permissions should use stronger authorization controls.

---

24. Permission Validation Flow

Every protected operation must follow this flow:

Request
   │
   ▼
Authenticate Client
   │
   ▼
Validate Session
   │
   ▼
Identify Connector
   │
   ▼
Identify Resource
   │
   ▼
Identify Operation
   │
   ▼
Check Permission
   │
   ├── DENIED → Stop
   │
   ▼
Security Validation
   │
   ▼
User Approval if Required
   │
   ▼
Execute Operation
   │
   ▼
Audit Operation
   │
   ▼
Return Result

---

25. Permission Denial

When permission is missing, the operation must not be executed.

Example response:

Permission Denied

Required Permission:
WRITE_FILE

Resource:
MyApplication/MainActivity.kt

Reason:
The client does not have permission to modify this file.

The system must not silently grant permission.

---

26. Resource Isolation

Permissions must be tied to authorized resources.

Example:

Allowed:
C:\Development\Projects\MyApplication

Denied:
C:\Windows
C:\Users\OtherUser
C:\Program Files

Resource boundaries must be validated before execution.

---

27. Permission Inheritance

Permission inheritance must be explicit.

For example:

Workspace
   │
   ├── Project A
   └── Project B

Permission to Workspace does not automatically mean unrestricted access to every operation inside every project.

Inheritance rules must be clearly defined by the permission policy.

---

28. Permission Conflicts

If multiple policies exist, the most restrictive applicable policy should win.

Example:

Policy A:
WRITE_FILE = ALLOW

Policy B:
WRITE_FILE = DENY

Result:

WRITE_FILE = DENY

Security must fail closed.

---

29. Permission Storage

Permission grants should be stored securely.

A permission record may contain:

PermissionId
ClientId
ConnectorId
ResourceId
Operation
Scope
GrantedBy
GrantedAt
ExpiresAt
Status

Sensitive authentication information must not be stored in plain text.

---

30. Permission Auditing

Permission events should be recorded.

Examples:

Permission Requested
Permission Granted
Permission Denied
Permission Revoked
Permission Expired
Permission Used

Audit logs must not contain passwords, tokens, or other secrets.

---

31. Permission Versioning

Permission definitions may evolve over time.

The protocol should support permission versions.

Example:

Permission Version:
1.0

Future versions may add or change permissions without breaking existing clients unnecessarily.

Security-sensitive changes must be versioned and documented.

---

32. Future Permission Model

The permission system is designed to support more detailed authorization in future versions.

Possible model:

WHO
 │
 ├── Client
 └── User

WHAT
 │
 ├── Connector
 ├── Resource
 └── Operation

WHERE
 │
 ├── Workspace
 ├── Project
 ├── Directory
 └── File

WHEN
 │
 ├── One Operation
 ├── Session
 └── Time Limited

APPROVAL
 │
 ├── Automatic
 └── User Confirmation

---

33. Security Principle

CodeType-Connector must follow:

Authentication
      ↓
Authorization
      ↓
Permission
      ↓
Scope
      ↓
Security Validation
      ↓
Approval
      ↓
Operation

The permission system must never be bypassed by a connector.

---

34. Final Permission Rules

The following rules are mandatory:

1. Default permissions are NONE.
2. Permissions must be explicitly granted.
3. Permissions must be scoped.
4. Sensitive operations require stronger authorization.
5. Clients cannot grant permissions to themselves.
6. Permission revocation must be supported.
7. Expired permissions must be rejected.
8. Unauthorized resources must be rejected.
9. Security failures must fail closed.
10. Every protected operation must be validated.
11. Connectors must not bypass the permission system.
12. The AI must never receive unrestricted computer access.

---

35. Current Status

Current project phase:

Permission System Documentation

The permission implementation will be developed after the protocol and API specifications are completed.

Future implementation will include:

- Permission models
- Permission requests
- Permission grants
- Permission validator
- Permission policies
- Permission storage
- Permission expiration
- Permission revocation
- Permission audit logging
- Automated security tests
