# backend-challenge

This repository was created as a possible solution for **BCA AG's Coding Challenge**.
It features a **REST API** for **managing file upload sessions** for users and clients.

## Setup

- Backend is built using **.NET 8.0**
- REST API services are testable using **Swagger**
- **Docker** support
- Testable using **unit tests**

## Resources

### (Business) Users
```
{
  "userID": int,
  "clients": Client[]
}
```
### Clients
```
{
  "clientID": int,
  "sessions": Session[]
}
```
### (File Upload) Sessions
```
{
  "sessionID": int,
  "userID": int,
  "clientID": int,
  "files": Dictionary<string, byte[]>
  "uploadStatus": UploadStatus
}
```
### Upload Status
  - an enum that can have the values `Pending`, `InProgress` or `Completed`
  - tracks if a file has successfully been uploaded to a session

## Workflow
1. A **new session** is created by a user for a client
   - The session is saved in a list of sessions as well as for the specified client
   - Status is set to `Pending`
2. The user **uploads files (PDF)** to the session
   - Each uploaded file is saved in the session's `files` dictionary
   - If the right file type is uploaded, the status is set to `Completed`
3. The user can **check a session's status** at any time

## Remarks
- Users and clients are **already predefined** for the sake of time,
but alternatively, they could both start as empty lists and then be filled
by sending POST requests to the API.
- All values exist **only during the application's lifecycle.**
  - I tried to implement a mock database using `Microsoft.EntityFrameworkCore`, but I did not have enough time.
- The only supported file type is **PDF**.
  - Other file types will send an **error response**
- A message is only returned _if a user tries to upload a file._
- If you are running a **Docker container**, you have to type in this URL to access the API: `localhost:8080/swagger`
