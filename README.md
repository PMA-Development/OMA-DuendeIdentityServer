# OMA-DuendeServer


# Description
A Duende IdentityServer designed to serve as the identity solution for GoGreen, enabling user authentication and authorization for both web and mobile clients, as well as secure access to the OMA-API.


# Project Details

| Platform            | GUI Framework | Timeframe | Database Solution         | Authentication Framework       |
|---------------------|---------------|-----------|---------------------------|--------------------------------|
| .NET 8 (ASP.NET Core)| Razor Pages   | November  | MS SQL (ConfigurationDb, PersistedGrantDb,ApplicationDb) | Duende IdentityServer (OIDC, OAuth2) |


# Getting Started

Follow these steps to set up and run the project locally:

### Prerequisites
- **SQL Server**: Ensure a running SQL Server instance for database connections.
- **Tools**: (Optional) Visual Studio or VS Code for development.

### Setup Instructions
1. Clone the Repository:
2. Set up the Appsettings: 
```json
{
  "IdentityServer": {
    "Authority": ""  //For its own API-endpoints
  },
  "ConnectionStrings": {
    "IdentityServerDatabase": "";
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
3. Run Migrations: update-database -context ApplicationDbContext (the other Context's get's applied themselfs on startup)
4. RUN the Application
5. For API testing and documentation, navigate to [http://localhost:5000/swagger](http://localhost:5000/swagger) (enabled only in Debug mode).
	

# Infomation

## ConfigurationDb Context
The **ConfigurationDbContext** stores configuration data for IdentityServer, including clients, identity/API resources, and API scopes. It allows managing registered clients, resources, and their available scopes.

read more about it [here](https://docs.duendesoftware.com/identityserver/v7/quickstarts/4_ef/)

## PersistedGrantDb Context
 
The **PersistedGrantDbContext** is responsible for storing data related to user sessions, including refresh tokens, authorization codes, and other grants required for maintaining persistent authentication and access.


read more about it [here](https://docs.duendesoftware.com/identityserver/v7/quickstarts/4_ef/)


## ApplicationDbContext

The **ApplicationDbContext** is responsible for managing user-related information in the database. It stores data such as users, roles, claims, and their relationships. This context integrates with ASP.NET Core Identity to provide essential user and role management functionality.


# NuGet Packages

[Duende IdentityServer 7.0.8](https://www.nuget.org/packages/Duende.IdentityServer/7.0.8#readme-body-tab) by Duende Software

[Duende IdentityServer.AspNetIdentity](https://www.nuget.org/packages/Duende.IdentityServer.AspNetIdentity/7.0.8#readme-body-tab) 7.0.8 by Duende Software

[Duende IdentityServer.EntityFramework](https://www.nuget.org/packages/Duende.IdentityServer.EntityFramework/7.0.8#readme-body-tab) 7.0.8 by Duende Software

[Microsoft.AspNetCore.Authentication.JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer/8.0.10#readme-body-tab) 8.0.10 by Microsoft

[Microsoft.AspNetCore.Identity.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.AspNetCore.Identity.EntityFrameworkCore/8.0.10#readme-body-tab) 8.0.10 by Microsoft

[Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/8.0.10#readme-body-tab) 8.0.10 by Microsoft

[Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/8.0.10#readme-body-tab) 8.0.10 by Microsoft

[Microsoft.EntityFrameworkCore.Tools 8.0.10](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/8.0.10#readme-body-tab) by Microsoft

[Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore/6.9.0#readme-body-tab) 6.9.0 by Swashbuckle Team

 


## API Documentation

 ### Swagger UI 
 This project includes **Swagger UI** for exploring and testing API endpoints. 
 **Access Swagger Documentation**: 
 - URL: [http://localhost:5000/swagger](http://localhost:5000/swagger) 
 - **Note**: Swagger is only enabled in the **Debug** environment for security purposes. It is not available in production.
## API Endpoints

| HTTP Method | Endpoint           | Description                                                                                 | Request Body         | Response Codes               |
|-------------|---------------------|---------------------------------------------------------------------------------------------|----------------------|------------------------------|
| POST        | /api/User/CreateUser     | Creates a new user with specified username and email. Assigns them to "Hotline-User" role. | `UserDTO`            | 200 OK, 400 Bad Request      |
| POST        | /api/User/UpdateUser     | Updates a user's information such as full name, phone, and password if provided.           | `UserDTO`            | 200 OK, 400 Bad Request, 404 Not Found |
| DELETE      | /api/User/DeleteUser     | Deletes a user based on the provided user ID.                                              | N/A                  | 200 OK, 400 Bad Request, 404 Not Found |
| PATCH       | /api/User/ToggleAdminRole| Toggles the "Admin" role for a specified user by adding or removing it.                    | N/A                  | 200 OK, 404 Not Found        |
| PUT         | /api/User/ResetPassword  | Resets a user's password to a new specified password.                                      | `id`, `newPassword`  | 200 OK, 400 Bad Request, 404 Not Found |
| GET         | /api/User/GetUsers       | Retrieves a list of all users with their IDs, emails, and phone numbers.                   | N/A                  | 200 OK                        |

### Debug Endpoints (only available in Debug mode)
these are only to see if the provided JWT token has the required Claims and if the they great read correctly
| HTTP Method | Endpoint           | Description                                                                                 | Request Body         | Response Codes               |
|-------------|---------------------|---------------------------------------------------------------------------------------------|----------------------|------------------------------|
| GET         | /api/User/claims    | Retrieves the claims of the current authenticated user.                                     | N/A                  | 200 OK                        |
| GET         | /api/User/role-check| Checks if the current user has the "Admin" role.                                            | N/A                  | 200 OK                        |

### Request Body - UserDTO Example

For endpoints that require a `UserDTO`, the JSON format should be as follows:

```json
{
  "Id": "Guid",
  "Email": "user@example.com",
  "Password": "Password123!",
  "Phone": "123456789",
  "FullName": "John Doe"
}

```
# appsettings.json Configuration

The `appsettings.json` file contains configuration settings for the Duende IdentityServer setup, database connections, logging, and allowed hosts. Here’s a breakdown of each section:

```json
{
  "IdentityServer": {
    "Authority": ""  //For its own API-endpoints
  },
  "ConnectionStrings": {
    "IdentityServerDatabase": "";"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```


# Roles and Permissions

This project uses a role-based authorization system to manage access to resources and functionality. The roles are specifically designed to serve different purposes across the **Duende IdentityServer API** and the **OMA-API**.

### **Roles**

| Role          | Description                                                                                   | Typical Actions                                                                                   |
|---------------|-----------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------|
| `hotline-user`| Default role for authenticated users. This role is specifically used in the **OMA-API**.      | Access features and endpoints within the OMA-API that are designed for regular user interactions.|
| `admin`       | A privileged role managed on the **Duende IdentityServer** for user administration tasks.     | Perform CRUD operations on users, manage roles, and configure user-related settings.             |

### **Usage in the Application**

#### **`hotline-user` Role**
- **Purpose**: 
  - This role is **not directly used in the Duende IdentityServer** but is passed to and utilized in the OMA-API.
  - Enables access to specific endpoints in the OMA-API designed for hotline users.
- **Assignment**:
  - Automatically assigned when a user is created on the IdentityServer.
  - Tokens issued for the OMA-API include this role in the claims to enforce role-based access control (RBAC).

#### **`admin` Role**
- **Purpose**:
  - Used in **Duende IdentityServer** for managing users and roles.
  - Allows access to administrative endpoints for creating, updating, and deleting users, as well as toggling roles.
- **Assignment**:
  - Assigned manually to specific users by administrators via the `/api/User/ToggleAdminRole` endpoint.
for hotline users.    

### Role-Based Access Workflow
1. **Duende IdentityServer**:
   - A user is created.
   - By default, the `hotline-user` role is assigned to new users.
   - Admin users can be elevated by assigning the `admin` role.
2. **OMA-API**:
   - Tokens issued by the IdentityServer include user roles (e.g., `hotline-user` or `admin`).
   - The OMA-API validates these tokens and enforces RBAC based on the `hotline-user` role for user-specific features.



## Clients Configuration

The following clients are configured in the **Duende IdentityServer** to support authentication for the web and mobile applications.

### **Clients**

| Client ID   | Client Name               | Grant Type     | PKCE Required | Allowed Scopes                               | Redirect URIs                                    | Post Logout Redirect URIs                       | CORS Origins                  | Access Token Lifetime | Refresh Tokens   |
|-------------|---------------------------|----------------|---------------|---------------------------------------------|------------------------------------------------|------------------------------------------------|-------------------------------|-----------------------|------------------|
| `OMA-Web`   | Web Client Application    | `code`         | Yes           | `openid`, `profile`, `email`, `role`       | `https://XXX/authentication/login-callback` | `https://XXX/authentication/logout-callback` | `N/A`      | `8 * 3600`            | N/A              |
| `OMA-Maui`  | MAUI Client Application   | `code`         | Yes           | `openid`, `profile`, `email`, `role`       | `myapp://auth`                                 | `myapp://auth`                                 | N/A                           | `8 * 3600`            | Sliding Expiration |

### **Details**

#### OMA-Web (Web Client Application)
- **Client Type**: Web application.
- **Grant Type**: Authorization Code (PKCE enabled).
- **Purpose**: Allows secure authentication for the web client.
- **Special Configurations**:
  - No client secret required.
  - PKCE (`RequirePkce`) is enabled for enhanced security.




#### OMA-Maui (MAUI Client Application)
- **Client Type**: Mobile application.
- **Grant Type**: Authorization Code (PKCE enabled).
- **Purpose**: Enables secure authentication for the mobile MAUI client.
- **Special Configurations**:
  - Supports offline access with refresh tokens.
  - Refresh tokens use a one-time policy with sliding expiration.

### Notes
- **PKCE (Proof Key for Code Exchange)**:
  - Enabled for both clients to enhance security in the Authorization Code flow.
  - Plain text PKCE is not allowed (`AllowPlainTextPkce = false`).
- **Access Token Lifetime**:
  - Defined by the `seconds` variable to control token expiration.
- **Refresh Tokens**:
  - Used only by the MAUI client for offline access and have sliding expiration (`TokenExpiration.Sliding`).

Notes:
- Client Secrets are not required because the Applications run's entirely on the user's device, making the Secret public, storing it in the client would make it public so it can be extracted
