CodeType-Connector — API Specification

1. Overview

CodeType-Connector API defines the service interfaces used by clients, AI systems, the CodeType-Connector server, and individual connectors.

The API provides controlled access to:

- Authentication
- Sessions
- Connectors
- Permissions
- Resources
- Operations
- Operation status
- User approval
- Cancellation
- Audit information

The API must always operate within the security and permission model defined by:

- "SECURITY.md"
- "PERMISSIONS.md"
- "PROTOCOL.md"

The API must never provide unrestricted access to the user's computer.

---

2. API Design Goals

The API must provide:

- Secure communication
- Authentication
- Session management
- Permission-based authorization
- Connector isolation
- Resource-level access control
- Operation validation
- Explicit approval for sensitive operations
- Structured responses
- Structured errors
- Operation tracking
- Cancellation
- Auditability
- Versioning
- Extensibility
- Backward compatibility

---

3. API Architecture

The logical architecture is:

AI System
    │
    ▼
CodeType-Connector Client
    │
    ▼
HTTP/API Request
    │
    ▼
CodeType-Connector Server
    │
    ├── Authentication
    │
    ├── Session Validation
    │
    ├── Connector Validation
    │
    ├── Operation Validation
    │
    ├── Resource Validation
    │
    ├── Permission Validation
    │
    ├── Security Validation
    │
    └── User Approval
    │
    ▼
Connector
    │
    ├── Visual Studio
    ├── File System
    └── Project Management
    │
    ▼
Operation
    │
    ▼
API Response

---

4. API Versioning

The initial API version is:

v1

Base path:

/api/v1

Example:

/api/v1/connectors

Future breaking API changes should use a new major version.

Example:

/api/v2

Compatible additions may be introduced within the same major version.

---

5. Authentication

Protected API operations require authentication.

Authentication must establish a valid client session.

Authentication Endpoint

POST /api/v1/auth/login

Example request:

{
  "clientId": "CodeType-Client",
  "authenticationMethod": "Local"
}

Example response:

{
  "success": true,
  "session": {
    "sessionId": "session-123",
    "clientId": "CodeType-Client",
    "status": "Active",
    "expiresAt": "2026-09-14T12:00:00Z"
  }
}

Authentication credentials must never be unnecessarily exposed to connectors.

---

6. Session

Sessions represent authenticated client access.

Session Information

A session may contain:

{
  "sessionId": "session-123",
  "clientId": "CodeType-Client",
  "status": "Active",
  "authenticatedAt": "2026-09-14T10:00:00Z",
  "expiresAt": "2026-09-14T12:00:00Z"
}

Possible session states:

ACTIVE
EXPIRED
REVOKED
INVALID

Expired or revoked sessions must be rejected.

---

6.1 Validate Session

GET /api/v1/auth/session

Example response:

{
  "success": true,
  "session": {
    "sessionId": "session-123",
    "status": "Active"
  }
}

---

6.2 Logout

POST /api/v1/auth/logout

Example response:

{
  "success": true
}

The session must become invalid after logout.

---

7. Connector API

Connectors provide controlled access to specific development environments or resources.

Initial connectors:

VisualStudio
FileSystem
ProjectManagement

---

7.1 List Connectors

GET /api/v1/connectors

Example response:

{
  "success": true,
  "connectors": [
    {
      "id": "VisualStudio",
      "name": "Visual Studio",
      "status": "Available",
      "version": "1.0"
    },
    {
      "id": "FileSystem",
      "name": "File System",
      "status": "Available",
      "version": "1.0"
    },
    {
      "id": "ProjectManagement",
      "name": "Project Management",
      "status": "Available",
      "version": "1.0"
    }
  ]
}

---

7.2 Connector Status

GET /api/v1/connectors/{connectorId}/status

Example:

GET /api/v1/connectors/VisualStudio/status

Example response:

{
  "success": true,
  "connector": {
    "id": "VisualStudio",
    "status": "Available"
  }
}

---

8. Permission API

Permissions control what an authenticated client is allowed to do.

The server must independently validate permissions.

A client must never be trusted simply because it claims to possess a permission.

---

8.1 List Available Permissions

GET /api/v1/permissions

Example response:

{
  "success": true,
  "permissions": [
    "READ_PROJECT",
    "READ_FILE",
    "CREATE_FILE",
    "WRITE_FILE",
    "DELETE_FILE",
    "BUILD_PROJECT",
    "RUN_PROJECT",
    "EXECUTE_COMMAND"
  ]
}

---

8.2 Check Permission

POST /api/v1/permissions/check

Example request:

{
  "connector": "VisualStudio",
  "operation": "BuildProject",
  "resource": {
    "type": "Project",
    "id": "MyApplication"
  },
  "permission": "BUILD_PROJECT"
}

Example response:

{
  "success": true,
  "authorized": true
}

If permission is not available:

{
  "success": false,
  "authorized": false,
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "The requested operation is not authorized."
  }
}

---

9. Permission Request

A client may request additional permissions.

POST /api/v1/permissions/request

Example:

{
  "connector": "FileSystem",
  "operation": "WriteFile",
  "resource": {
    "type": "File",
    "path": "C:\\Development\\MyApplication\\README.md"
  },
  "permission": "WRITE_FILE",
  "scope": "FILE"
}

Example response:

{
  "success": true,
  "requestId": "permission-request-123",
  "status": "APPROVAL_REQUIRED"
}

The permission must not automatically be granted merely because it was requested.

---

10. Permission Grant

A permission may be granted only after the required authorization and approval process.

POST /api/v1/permissions/grant

Example:

{
  "requestId": "permission-request-123",
  "decision": "ALLOW"
}

Possible decisions:

ALLOW
DENY

---

11. Permission Revocation

Permissions must be revocable.

POST /api/v1/permissions/revoke

Example:

{
  "permissionId": "permission-123"
}

Revoked permissions must immediately stop authorizing new operations.

---

12. Resource API

Resources represent projects, files, solutions, applications, and other controlled objects.

---

12.1 List Authorized Resources

GET /api/v1/resources

Only resources accessible under the current session and permissions may be returned.

---

12.2 Resource Information

GET /api/v1/resources/{resourceId}

The server must validate that the requesting client is authorized to access the resource.

---

13. Operation API

Operations are the primary mechanism for performing actions through connectors.

General endpoint:

POST /api/v1/operations

Example:

{
  "protocolVersion": "1.0",
  "requestId": "12345678",
  "connector": "VisualStudio",
  "operation": "ReadProject",
  "resource": {
    "type": "Project",
    "id": "MyApplication"
  },
  "permission": "READ_PROJECT",
  "parameters": {}
}

---

14. Operation Processing

Every protected operation must follow this order:

Receive Request
      │
      ▼
Validate API Version
      │
      ▼
Authenticate
      │
      ▼
Validate Session
      │
      ▼
Validate Connector
      │
      ▼
Validate Operation
      │
      ▼
Validate Resource
      │
      ▼
Validate Permission
      │
      ▼
Security Validation
      │
      ▼
User Approval
      │
      ▼
Execute Operation
      │
      ▼
Audit
      │
      ▼
Return Response

A failed validation must stop further processing.

---

15. Read File Operation

POST /api/v1/operations

Example request:

{
  "protocolVersion": "1.0",
  "requestId": "read-file-001",
  "connector": "FileSystem",
  "operation": "ReadFile",
  "resource": {
    "type": "File",
    "path": "C:\\Development\\MyApplication\\README.md"
  },
  "permission": "READ_FILE"
}

Example response:

{
  "protocolVersion": "1.0",
  "requestId": "read-file-001",
  "success": true,
  "result": {
    "path": "README.md",
    "content": "Project documentation"
  }
}

The server must validate the path before reading the file.

---

16. Write File Operation

POST /api/v1/operations

Example:

{
  "protocolVersion": "1.0",
  "requestId": "write-file-001",
  "connector": "FileSystem",
  "operation": "WriteFile",
  "resource": {
    "type": "File",
    "path": "C:\\Development\\MyApplication\\README.md"
  },
  "permission": "WRITE_FILE",
  "parameters": {
    "content": "Updated documentation"
  }
}

Processing:

Authentication
      ↓
Session Validation
      ↓
Permission Validation
      ↓
Path Validation
      ↓
Security Validation
      ↓
User Approval
      ↓
Write

---

17. Create File Operation

Operation:
CreateFile

Permission:
CREATE_FILE

Example:

{
  "connector": "FileSystem",
  "operation": "CreateFile",
  "resource": {
    "type": "File",
    "path": "C:\\Development\\MyApplication\\NewFile.cs"
  },
  "permission": "CREATE_FILE"
}

---

18. Delete File Operation

Delete operations are high-risk.

Operation:
DeleteFile

Permission:
DELETE_FILE

Deletion must require appropriate authorization and, where configured, explicit user approval.

---

19. Visual Studio API

The Visual Studio connector provides controlled access to Visual Studio.

Initial supported operations may include:

ReadProject
OpenProject
CreateProject
ModifyProject
BuildProject
RebuildProject
CleanProject
RestoreProject
RunProject
DebugProject

Every operation must be independently authorized.

---

20. Visual Studio Read Project

POST /api/v1/operations

Example:

{
  "connector": "VisualStudio",
  "operation": "ReadProject",
  "resource": {
    "type": "Project",
    "id": "MyApplication"
  },
  "permission": "VISUAL_STUDIO_READ"
}

---

21. Visual Studio Open Project

Permission:

VISUAL_STUDIO_OPEN_PROJECT

Example:

{
  "connector": "VisualStudio",
  "operation": "OpenProject",
  "resource": {
    "type": "Project",
    "id": "MyApplication"
  },
  "permission": "VISUAL_STUDIO_OPEN_PROJECT"
}

---

22. Visual Studio Create Project

Permission:

VISUAL_STUDIO_CREATE_PROJECT

Example:

{
  "connector": "VisualStudio",
  "operation": "CreateProject",
  "resource": {
    "type": "Directory",
    "path": "C:\\Development\\Projects"
  },
  "permission": "VISUAL_STUDIO_CREATE_PROJECT",
  "parameters": {
    "projectType": "Console",
    "name": "MyNewApplication"
  }
}

Project creation must be restricted to authorized directories.

---

23. Visual Studio Modify Project

Permission:

VISUAL_STUDIO_MODIFY_PROJECT

The operation may modify project configuration or source files only within the authorized project scope.

---

24. Visual Studio Build

Permission:

VISUAL_STUDIO_BUILD

Example:

{
  "connector": "VisualStudio",
  "operation": "BuildProject",
  "resource": {
    "type": "Project",
    "id": "MyApplication"
  },
  "permission": "VISUAL_STUDIO_BUILD",
  "parameters": {
    "configuration": "Debug"
  }
}

---

25. Visual Studio Run

Permission:

VISUAL_STUDIO_RUN

Running an application is a privileged operation.

The connector must not execute a project without successful authorization.

---

26. Visual Studio Debug

Permission:

VISUAL_STUDIO_DEBUG

Debugging operations may require additional security controls because they can interact with running processes and development environments.

---

27. Project Management API

Project management operations may include:

CreateProject
ReadProject
ModifyProject
DeleteProject
AddFile
RemoveFile
RenameFile
MoveFile

These operations must remain within the authorized project scope.

---

28. Command Execution

Command execution is a high-risk capability.

Endpoint:

POST /api/v1/operations

Example:

{
  "connector": "ProjectManagement",
  "operation": "ExecuteCommand",
  "resource": {
    "type": "Project",
    "id": "MyApplication"
  },
  "permission": "EXECUTE_COMMAND",
  "parameters": {
    "command": "dotnet",
    "arguments": [
      "build"
    ]
  }
}

Command execution must include:

- Permission validation
- Command allowlisting or policy validation
- Argument validation
- Resource/workspace validation
- Timeout
- Resource limits
- Audit logging

The API must never interpret "EXECUTE_COMMAND" as unrestricted operating-system access.

---

29. Operation Response

Successful operation:

{
  "protocolVersion": "1.0",
  "requestId": "operation-001",
  "success": true,
  "result": {}
}

Failed operation:

{
  "protocolVersion": "1.0",
  "requestId": "operation-001",
  "success": false,
  "error": {
    "code": "OPERATION_FAILED",
    "message": "The operation could not be completed."
  }
}

---

30. Operation Status

Long-running operations receive an operation identifier.

GET /api/v1/operations/{operationId}

Example response:

{
  "success": true,
  "operation": {
    "operationId": "operation-123",
    "status": "RUNNING",
    "progress": 45
  }
}

Possible states:

PENDING
APPROVAL_REQUIRED
AUTHORIZED
RUNNING
COMPLETED
FAILED
CANCELLED
TIMEOUT

---

31. Cancel Operation

Long-running operations should support cancellation.

POST /api/v1/operations/{operationId}/cancel

Example response:

{
  "success": true,
  "operationId": "operation-123",
  "status": "CANCELLED"
}

Cancellation must not leave the system in an unsafe or inconsistent state.

---

32. User Approval API

Sensitive operations may require explicit user approval.

GET /api/v1/approvals

Example approval request:

{
  "approvalId": "approval-123",
  "operationId": "operation-123",
  "operation": "WriteFile",
  "resource": "MainActivity.kt",
  "permission": "WRITE_FILE",
  "status": "PENDING"
}

---

32.1 Approve Operation

POST /api/v1/approvals/{approvalId}/allow

---

32.2 Deny Operation

POST /api/v1/approvals/{approvalId}/deny

A denied operation must never execute.

---

33. Audit API

Security-relevant operations should generate audit records.

GET /api/v1/audit

Example audit record:

{
  "requestId": "operation-001",
  "clientId": "CodeType-Client",
  "sessionId": "session-123",
  "connector": "VisualStudio",
  "operation": "BuildProject",
  "resource": "MyApplication",
  "permission": "VISUAL_STUDIO_BUILD",
  "timestamp": "2026-09-14T10:30:00Z",
  "result": "SUCCESS",
  "approvalStatus": "APPROVED"
}

Audit records must not contain:

- Passwords
- Access tokens
- API keys
- Private keys
- Secret environment variables
- Sensitive file contents

---

34. Error Model

All API errors should use a consistent structure.

{
  "success": false,
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "The requested operation is not authorized."
  },
  "requestId": "12345678"
}

Initial error codes:

AUTHENTICATION_REQUIRED
AUTHENTICATION_FAILED
SESSION_EXPIRED
SESSION_REVOKED

PERMISSION_REQUIRED
PERMISSION_DENIED
PERMISSION_EXPIRED

INVALID_REQUEST
INVALID_API_VERSION
INVALID_PROTOCOL_VERSION

INVALID_CONNECTOR
INVALID_OPERATION
INVALID_RESOURCE

RESOURCE_NOT_FOUND
RESOURCE_ACCESS_DENIED

SECURITY_VALIDATION_FAILED
PATH_ACCESS_DENIED

APPROVAL_REQUIRED
APPROVAL_DENIED

OPERATION_FAILED
OPERATION_TIMEOUT
OPERATION_CANCELLED

CONNECTOR_UNAVAILABLE
CONNECTOR_ERROR

RATE_LIMITED
REQUEST_TOO_LARGE
RESPONSE_TOO_LARGE

INTERNAL_ERROR

---

35. HTTP Status Codes

Suggested HTTP status mapping:

200 OK
201 Created
202 Accepted

400 Bad Request
401 Unauthorized
403 Forbidden
404 Not Found
409 Conflict
413 Payload Too Large
429 Too Many Requests

500 Internal Server Error
503 Service Unavailable

The API error body must still use the standard CodeType-Connector error format.

---

36. Request Limits

The API must enforce configurable limits.

Examples:

Maximum request size
Maximum file content size
Maximum response size
Maximum command argument size
Maximum operation duration
Maximum concurrent operations
Maximum requests per client

Limits should protect against accidental or malicious resource exhaustion.

---

37. Path Security

Filesystem APIs must validate all paths.

The API must prevent:

Path Traversal
Unauthorized Directory Access
Unauthorized File Access
Symbolic Link Escape
Directory Escape

Examples of dangerous input:

..\..\secret.txt

or equivalent encoded/path-normalized variants.

The final normalized path must remain within the authorized scope.

---

38. Resource Isolation

Each operation must be associated with an authorized resource.

Example:

Authorized Project:
MyApplication

Requested Project:
OtherApplication

Result:
RESOURCE_ACCESS_DENIED

A permission for one project must not automatically grant access to another project.

---

39. Connector Isolation

Connectors must operate within their assigned security boundary.

For example:

VisualStudio Connector
        │
        └── Visual Studio resources

FileSystem Connector
        │
        └── Authorized filesystem resources

ProjectManagement Connector
        │
        └── Authorized project resources

One connector must not bypass another connector's security controls.

---

40. Rate Limiting

The API should support rate limiting.

Limits may be applied by:

Client
Session
Connector
Operation
Resource

Example error:

{
  "success": false,
  "error": {
    "code": "RATE_LIMITED",
    "message": "Too many requests."
  }
}

---

41. Idempotency

Operations that may be retried should support idempotency where appropriate.

Examples:

CREATE_PROJECT
CREATE_FILE
DELETE_FILE
GIT_COMMIT
GIT_PUSH

The implementation must define which operations are safe to retry.

A duplicate request must not unintentionally perform a destructive operation twice.

---

42. Request Correlation

Every request should contain:

requestId

The same identifier must be returned in the response.

This allows:

- Debugging
- Logging
- Auditing
- Error tracking
- Operation correlation

---

43. API Security Requirements

The following rules are mandatory:

1. Protected endpoints require authentication.
2. Protected operations require authorization.
3. Permissions must be independently validated by the server.
4. Resource scope must always be validated.
5. Connectors must not bypass authorization.
6. Sensitive operations require appropriate approval.
7. Expired sessions must be rejected.
8. Revoked sessions must be rejected.
9. Expired permissions must be rejected.
10. Invalid resources must be rejected.
11. Invalid paths must be rejected.
12. High-risk commands require additional validation.
13. Security failures must fail closed.
14. Sensitive information must not be exposed.
15. Requests must have reasonable size limits.
16. Operations must have appropriate timeouts.
17. Audit records must not contain secrets.
18. API version compatibility must be validated.
19. The API must never provide unrestricted computer access.

---

44. Example End-to-End Visual Studio Request

Scenario:

AI wants to modify a project file in Visual Studio.

AI System
    │
    ▼
CodeType-Connector Client
    │
    ▼
POST /api/v1/operations
    │
    ▼
Authentication
    │
    ▼
Session Validation
    │
    ▼
VisualStudio Connector Validation
    │
    ▼
WriteFile Operation Validation
    │
    ▼
Resource Validation
    │
    ▼
WRITE_FILE Permission Validation
    │
    ▼
Path Security Validation
    │
    ▼
User Approval
    │
    ▼
VisualStudio Connector
    │
    ▼
Project File
    │
    ▼
Operation Result
    │
    ▼
Audit
    │
    ▼
API Response

At no point should the AI receive unrestricted access to the operating system.

---

45. Future API Capabilities

Future versions may add:

- File upload
- File download
- Chunked file transfer
- Streaming
- Build output streaming
- Compiler output streaming
- Process monitoring
- Git operations
- GitHub operations
- Docker operations
- CI/CD operations
- Visual Studio Code connector
- JetBrains connector
- Android Studio connector
- Remote development
- Enterprise authentication
- Multi-user permissions
- Organization policies
- Advanced audit systems

Future features must follow the same permission and security architecture.

---

46. API Implementation Principle

The API is a controlled interface, not a general-purpose remote shell.

The security boundary is:

AI
 │
 ▼
API
 │
 ▼
Authentication
 │
 ▼
Authorization
 │
 ▼
Permission
 │
 ▼
Resource Scope
 │
 ▼
Security Validation
 │
 ▼
User Approval
 │
 ▼
Connector
 │
 ▼
Authorized Operation

Every layer must enforce its own responsibility.

---

47. Current Implementation Status

Current phase:

API Specification
      ↓
Documentation
      ↓
Architecture Validation
      ↓
Core Implementation
      ↓
Protocol Implementation
      ↓
Security Implementation
      ↓
Permission Implementation
      ↓
Server Implementation
      ↓
Connector Implementation
      ↓
Testing

The API specification defines the intended contracts.

Actual implementation will be introduced only after the documentation and architecture have been finalized.

---

48. Final Principle

CodeType-Connector must follow one fundamental rule:

«AI capability must never equal unrestricted computer access.»

Every operation must pass through:

Authentication
      ↓
Authorization
      ↓
Permission
      ↓
Resource Scope
      ↓
Security Validation
      ↓
User Approval
      ↓
Connector
      ↓
Operation

If any required security check fails:

STOP

No unauthorized operation may continue.
