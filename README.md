# Management Web App API (ASP.NET Backend)

ASP.NET Core 9.0 Web API with Entity Framework and SQL Server for user management system.

## ??? Technology Stack

- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core 9.0.6
- SQL Server (LocalDB)
- BCrypt.Net for password hashing

## ??? Database Configuration

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source={your_server};Initial Catalog={your_database_name};Integrated Security=True;Pooling=False;Encrypt=False;Trust Server Certificate=True"
}
```

**Replace with your values:**
- `{your_server}` - Your SQL Server instance (e.g., `(localdb)\\MSSQLLocalDB`)
- `{your_database_name}` - Your database name

## ?? API Endpoints

### Permissions API
- `GET /api/permissions` - Get all permissions
- `POST /api/permissions` - Create permission

### Roles API  
- `GET /api/roles` - Get all roles
- `POST /api/roles` - Create role

### Users API
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create user
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user
- `POST /api/users/DataTable` - Get paginated users with search/sort

## ?? Prerequisites

- .NET 9.0 SDK
- SQL Server LocalDB
- Visual Studio 2022 (recommended)

## ?? Running the API

```bash
# Restore packages
dotnet restore

# Run database migrations
dotnet ef database update

# Start the API
dotnet run
```

**API URLs:**
- HTTP: `http://localhost:5163`
- HTTPS: `https://localhost:7166`

## ?? API Examples

### Create User
```http
POST /api/users
Content-Type: application/json

{
  "id": "{USER_ID}",
  "firstName": "{First Name}",
  "lastName": "{Last Name}",
  "email": "{email@domain.com}",
  "phone": "{phone_number}",
  "roleId": "{ROLE_ID}",
  "username": "{username}",
  "password": "{password}",
  "permission": [
    {
      "permissionId": "{PERM_ID}",
      "isReadable": true,
      "isWritable": true,
      "isDeletable": true
    }
  ]
}
```

### Update User
```http
PUT /api/users/{USER_ID}
Content-Type: application/json

{
  "firstName": "{Updated First Name}",
  "lastName": "{Updated Last Name}",
  "email": "{updated.email@domain.com}",
  "phone": "{updated_phone}",
  "roleId": "{NEW_ROLE_ID}",
  "username": "{updated_username}",
  "password": "{new_password}",
  "permission": [
    {
      "permissionId": "{PERM_ID}",
      "isReadable": true,
      "isWritable": false,
      "isDeletable": false
    }
  ]
}
```


### Permission Object
```json
{
  "permissionId": "{PERM_ID}",
  "permissionName": "{Permission Name}"
}
```

### Role Object
```json
{
  "roleId": "{ROLE_ID}", 
  "roleName": "{Role Name}"
}
```

## ?? Features

- **CRUD Operations** for Users, Roles, and Permissions
- **Password Hashing** with BCrypt
- **Pagination & Search** via DataTable endpoint
- **Role-based Access Control** with granular permissions
- **Entity Framework** with SQL Server integration

---

**Note**: Ensure SQL Server LocalDB is running before starting the API.