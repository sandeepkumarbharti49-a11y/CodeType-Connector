CodeType-Connector — Communication Protocol

1. Overview

CodeType-Connector Protocol defines how an AI system, client application, CodeType-Connector server, and individual connectors communicate with each other.

The protocol provides a common and versioned communication layer for:

- Request handling
- Response handling
- Authentication
- Authorization
- Permission validation
- Connector selection
- Resource identification
- Operation execution
- Error handling
- Audit information

The protocol is designed to allow future connectors to be added without changing the core communication architecture.

---

2. Protocol Goals

The protocol must provide:

- Clear request and response formats
- Version compatibility
- Secure communication
- Authentication support
- Permission-aware requests
- Connector identification
- Resource identification
- Operation identification
- Structured errors
- Request tracking
- Auditability
- Extensibility
- Backward compatibility

---

3. Protocol Architecture

The communication flow is:

AI System
    │
    ▼
CodeType-Connector Client
    │
    ▼
Protocol Request
    │
    ▼
CodeType-Connector Server
    │
    ├── Authentication
    │
    ├── Session Validation
    │
    ├── Permission Validation
    │
    └── Security Validation
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
Protocol Response
    │
    ▼
Client
    │
    ▼
AI System

---

4. Protocol Version

Every protocol request should contain a protocol version.

Example:

protocolVersion: "1.0"

The protocol version allows future versions to introduce new features while maintaining compatibility with existing clients where possible.

---

5. Request Identifier

Every request should have a unique request identifier.

Example:

requestId: "8b7d1c3e-7b21-4a7e-9c55-123456789abc"

The request ID is used for:

- Request tracking
- Response matching
- Logging
- Debugging
- Auditing
- Error reporting

A response must contain the corresponding request ID.

---

6. Basic Request Structure

A protocol request should contain the following information:

{
  "protocolVersion": "1.0",
  "requestId": "unique-request-id",
  "connector": "VisualStudio",
  "operation": "ReadProject",
  "resource": {
    "type": "Project",
    "id": "MyApplication"
  },
  "permission": "READ_PROJECT"
}

Additional fields may be added as required.

---

7. Request Components

7.1 protocolVersion

Identifies the protocol version.

Example:

"protocolVersion": "1.0"

---

7.2 requestId

Unique identifier for the request.

Example:

"requestId": "12345678-abcd-1234-abcd-123456789abc"

---

7.3 connector

Identifies the connector that should handle the operation.

Examples:

"VisualStudio"
"FileSystem"
"ProjectManagement"

---

7.4 operation

Identifies the requested operation.

Examples:

"ReadProject"
"ReadFile"
"WriteFile"
"CreateProject"
"BuildProject"
"RunProject"

---

7.5 resource

Identifies the resource affected by the operation.

Example:

"resource": {
  "type": "Project",
  "id": "MyApplication"
}

---

7.6 permission

Identifies the permission required for the operation.

Example:

"permission": "READ_PROJECT"

The server must independently validate the permission.

A client must not be trusted simply because it claims to have a permission.

---

8. Authentication Information

Protected requests must be associated with an authenticated session.

Authentication information should be handled through the secure connection/session mechanism rather than exposing credentials inside ordinary operation payloads.

Conceptually:

Client
  │
  ▼
Authentication
  │
  ▼
Session
  │
  ▼
Protocol Requests

Authentication credentials must never be unnecessarily exposed to connectors or the AI.

---

9. Session

An authenticated client operates through a session.

A session may contain:

sessionId
clientId
authenticatedAt
expiresAt
permissions
status

Example:

{
  "sessionId": "session-123",
  "clientId": "AI-Client",
  "status": "Active"
}

Expired or revoked sessions must be rejected.

---

10. Operation Request

An operation request may include operation-specific data.

Example:

{
  "protocolVersion": "1.0",
  "requestId": "12345678",
  "connector": "FileSystem",
  "operation": "ReadFile",
  "resource": {
    "type": "File",
    "path": "C:\\Development\\Projects\\MyApplication\\README.md"
  },
  "permission": "READ_FILE",
  "parameters": {}
}

The server must validate the path and permission before accessing the file.

---

11. Write Operation

A write request may contain content.

Example:

{
  "protocolVersion": "1.0",
  "requestId": "12345679",
  "connector": "FileSystem",
  "operation": "WriteFile",
  "resource": {
    "type": "File",
    "path": "C:\\Development\\Projects\\MyApplication\\README.md"
  },
  "permission": "WRITE_FILE",
  "parameters": {
    "content": "Updated content"
  }
}

Write operations must pass:

Authentication
      ↓
Permission Validation
      ↓
Path Validation
      ↓
Security Validation
      ↓
User Approval if Required
      ↓
Write Operation

---

12. Build Request

A project build request may look like:

{
  "protocolVersion": "1.0",
  "requestId": "12345680",
  "connector": "VisualStudio",
  "operation": "BuildProject",
  "resource": {
    "type": "Project",
    "id": "MyApplication"
  },
  "permission": "BUILD_PROJECT",
  "parameters": {
    "configuration": "Debug"
  }
}

The connector must verify that the client has the required permission before starting the build.

---

13. Run Request

Running an application is a privileged operation.

Example:

{
  "protocolVersion": "1.0",
  "requestId": "12345681",
  "connector": "VisualStudio",
  "operation": "RunProject",
  "resource": {
    "type": "Project",
    "id": "MyApplication"
  },
  "permission": "RUN_PROJECT"
}

Execution must not occur if authorization fails.

---

14. Command Request

Command execution is a high-risk capability.

Example:

{
  "protocolVersion": "1.0",
  "requestId": "12345682",
  "connector": "ProjectManagement",
  "operation": "ExecuteCommand",
  "resource": {
    "type": "Project",
    "id": "MyApplication"
  },
  "permission": "EXECUTE_COMMAND",
  "parameters": {
    "command": "dotnet",
    "arguments": ["build"]
  }
}

Command execution must be separately authorized and validated.

The connector must not assume that a requested command is safe.

---

15. Response Structure

Every request should receive a structured response.

Example:

{
  "protocolVersion": "1.0",
  "requestId": "12345678",
  "success": true,
  "result": {}
}

---

16. Successful Response

Example:

{
  "protocolVersion": "1.0",
  "requestId": "12345678",
  "success": true,
  "result": {
    "projectName": "MyApplication",
    "projectType": ".NET"
  }
}

---

17. Error Response

Errors must use a structured format.

Example:

{
  "protocolVersion": "1.0",
  "requestId": "12345678",
  "success": false,
  "error": {
    "code": "PERMISSION_DENIED",
    "message": "The requested operation is not authorized."
  }
}

---

18. Standard Error Codes

Initial error codes include:

AUTHENTICATION_REQUIRED
AUTHENTICATION_FAILED
SESSION_EXPIRED
SESSION_REVOKED

PERMISSION_REQUIRED
PERMISSION_DENIED
PERMISSION_EXPIRED

INVALID_REQUEST
INVALID_PROTOCOL_VERSION
INVALID_CONNECTOR
INVALID_OPERATION
INVALID_RESOURCE

RESOURCE_NOT_FOUND
RESOURCE_ACCESS_DENIED

SECURITY_VALIDATION_FAILED
PATH_ACCESS_DENIED

OPERATION_FAILED
OPERATION_TIMEOUT

CONNECTOR_UNAVAILABLE
CONNECTOR_ERROR

RATE_LIMITED
INTERNAL_ERROR

Additional error codes may be added in future versions.

---

19. Error Security

Error messages must not expose sensitive information.

The protocol must avoid returning:

- Passwords
- Access tokens
- API keys
- Private keys
- Secret environment variables
- Internal security configuration
- Unnecessary system information
- Sensitive filesystem information

Detailed diagnostic information should remain in controlled logs.

---

20. Permission Validation

The server must validate the permission independently.

Example:

Request:
WRITE_FILE

Client Permission:
READ_FILE

Result:
PERMISSION_DENIED

The request must not be executed.

---

21. Resource Validation

The requested resource must be validated.

Example:

Authorized Project:
MyApplication

Requested Resource:
OtherApplication

Result:

RESOURCE_ACCESS_DENIED

---

22. Operation Validation

The operation must be supported by the selected connector.

Example:

Connector:
VisualStudio

Operation:
UnknownOperation

Result:

INVALID_OPERATION

---

23. Connector Validation

The server must verify that the requested connector exists and is available.

Example:

Connector:
VisualStudio

If the connector is unavailable:

CONNECTOR_UNAVAILABLE

must be returned.

---

24. User Approval

Some protocol operations require user approval.

The protocol may represent an approval request such as:

{
  "type": "ApprovalRequired",
  "requestId": "12345683",
  "operation": "WriteFile",
  "resource": "MainActivity.kt",
  "permission": "WRITE_FILE"
}

The user may respond:

ALLOW

or:

DENY

No sensitive operation should proceed without required approval.

---

25. Operation Status

Long-running operations may require status tracking.

Possible states:

PENDING
APPROVAL_REQUIRED
AUTHORIZED
RUNNING
COMPLETED
FAILED
CANCELLED
TIMEOUT

Example:

Request
   │
   ▼
PENDING
   │
   ▼
APPROVAL_REQUIRED
   │
   ▼
AUTHORIZED
   │
   ▼
RUNNING
   │
   ▼
COMPLETED

---

26. Cancellation

Long-running operations should support cancellation where technically possible.

Example:

Request
   │
   ▼
RUNNING
   │
   ▼
CANCEL
   │
   ▼
CANCELLED

Cancellation must not leave the system in an unsafe state.

---

27. Timeouts

Operations should have appropriate timeouts.

Timeouts help protect against:

- Hung processes
- Unresponsive connectors
- Resource exhaustion
- Accidental infinite operations

A timeout should produce:

OPERATION_TIMEOUT

where appropriate.

---

28. Idempotency

Where appropriate, requests should support idempotency.

This helps prevent accidental duplicate operations caused by retries.

Especially important for operations such as:

CREATE_PROJECT
CREATE_FILE
DELETE_FILE
GIT_COMMIT
GIT_PUSH

The implementation should define which operations are safe to retry.

---

29. Request Size Limits

The protocol should enforce reasonable request limits.

Examples:

Maximum request size
Maximum file content size
Maximum parameter size
Maximum response size

Limits should be configurable where appropriate.

---

30. Response Size Limits

Responses should also be subject to size limits.

Large resources should use controlled mechanisms such as:

- Pagination
- Chunking
- Streaming
- File references

The protocol should avoid unnecessarily loading very large files into memory.

---

31. File Transfer

Future protocol versions may support file transfer.

Possible operations:

UPLOAD_FILE
DOWNLOAD_FILE
READ_CHUNK
WRITE_CHUNK

File transfer must still respect:

- Authentication
- Authorization
- Permission scope
- Path restrictions
- Size limits
- Security validation

---

32. Streaming

Long-running operations may use streaming responses.

Examples:

Build output
Compiler messages
Process output
Progress information
Logs

Streaming must remain authenticated and permission controlled.

---

33. Protocol Extensibility

The protocol must be extensible.

Future capabilities may include:

- New connectors
- New operations
- New permission types
- New resource types
- Streaming
- Remote development
- Enterprise authentication
- Distributed connectors

New functionality should not require redesigning the complete protocol.

---

34. Backward Compatibility

Protocol changes should preserve compatibility where possible.

Breaking changes should require a new major protocol version.

Example:

1.x

may introduce compatible additions.

A major incompatible change may become:

2.0

Clients and servers should negotiate supported versions.

---

35. Version Negotiation

A client may advertise supported protocol versions.

Example:

Client:
Supported Versions
1.0
1.1

Server:

Supported Versions
1.0

Negotiated version:

1.0

If no compatible version exists:

INVALID_PROTOCOL_VERSION

---

36. Audit Information

Protocol operations should provide enough metadata for security auditing.

Possible audit fields:

requestId
clientId
sessionId
connector
operation
resource
permission
timestamp
result
approvalStatus

Sensitive credentials must never be included.

---

37. Security Processing Order

The server must process protected requests in a secure order:

Receive Request
      │
      ▼
Validate Protocol
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
Execute
      │
      ▼
Audit
      │
      ▼
Response

A failed validation must stop further processing.

---

38. Protocol Security Rules

The following rules are mandatory:

1. Every protected request must be authenticated.
2. Every protected request must be authorized.
3. Every protected operation must have the required permission.
4. Resource scope must be validated.
5. Connectors must not bypass permission checks.
6. Sensitive operations require appropriate approval.
7. Invalid requests must be rejected.
8. Expired sessions must be rejected.
9. Expired permissions must be rejected.
10. Security failures must fail closed.
11. Sensitive information must not be exposed.
12. Every request should have a unique request ID.
13. Every response must reference its request ID.
14. Protocol versions must be validated.
15. High-risk operations must receive additional security controls.

---

39. Example End-to-End Request

Example scenario:

AI wants to modify a Visual Studio project file.

AI
 │
 │ Request: WriteFile
 ▼
Client
 │
 ▼
CodeType-Connector
 │
 ├── Authenticate
 │
 ├── Validate Session
 │
 ├── Validate VisualStudio Connector
 │
 ├── Validate WriteFile Operation
 │
 ├── Check WRITE_FILE
 │
 ├── Validate Project Scope
 │
 ├── Validate File Path
 │
 ├── Security Validation
 │
 ├── User Approval
 │
 ▼
VisualStudio Connector
 │
 ▼
Visual Studio Project
 │
 ▼
Operation Result
 │
 ▼
Audit Log
 │
 ▼
Response
 │
 ▼
AI

---

40. Final Protocol Principle

The CodeType-Connector protocol follows:

REQUEST
   ↓
AUTHENTICATE
   ↓
AUTHORIZE
   ↓
PERMISSION
   ↓
VALIDATE
   ↓
APPROVE
   ↓
EXECUTE
   ↓
AUDIT
   ↓
RESPONSE

The protocol exists to ensure that AI capabilities remain controlled, observable, and permission-based.

The protocol must never become a mechanism for unrestricted computer access.

---

41. Current Status

Current project phase:

Protocol Documentation

The protocol implementation will be developed after the protocol and API design have been reviewed.

Next implementation stages will include:

- Protocol models
- Request models
- Response models
- Error models
- Serialization
- Version negotiation
- Request validation
- Protocol tests
- Client/server communication
