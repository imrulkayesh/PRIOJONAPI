# Clean Architecture Implementation - User Management

This document summarizes the changes made to implement clean architecture principles in the BACKBONE API project, specifically addressing the concern about controllers directly accessing the database.

## Issues Addressed

1. **Controllers directly accessing database**: Controllers were directly executing SQL queries, violating clean architecture principles
2. **Lack of proper separation of concerns**: Database access logic was mixed with presentation logic
3. **Missing UserRepository**: No proper repository pattern implementation for user-related operations

## Changes Made

### 1. Created IUserRepository Interface

**File**: `BACKBONE.Application/Interfaces/IUserRepository.cs`

```csharp
public interface IUserRepository
{
    Task<TNDR_USER> GetUserByEmailAsync(string email);
    Task<JWT_USER_DTO> GetJwtUserByIdAsync(string userId);
}
```

### 2. Created UserRepository Implementation

**File**: `BACKBONE.Infrastructure/UserRepository.cs`

This implementation handles all database operations related to users:
- Retrieving user by email for authentication
- Retrieving JWT user information by ID for token generation

### 3. Updated IUnitOfWork Interface

**File**: `BACKBONE.Application/Interfaces/IUnitOfWork.cs`

Added UserRepository and HrisAuthService to the UnitOfWork:
```csharp
public interface IUnitOfWork
{
    ITokenRepository TokenRepositor { get; }
    IRefreshTokenService RefreshTokenService { get; }
    IJwtTokenService JwtTokenService { get; }
    IRfqRepository RfqRepository { get; }
    IUserRepository UserRepository { get; }
    IHrisAuthService HrisAuthService { get; }
}
```

### 4. Updated UnitOfWork Implementation

**File**: `BACKBONE.Infrastructure/UnitOfWork.cs`

Updated constructor and properties to include the new services.

### 5. Updated Service Registration

**File**: `BACKBONE.API/Services/ServiceExtension.cs`

Registered the new UserRepository with the DI container.

### 6. Refactored AuthController

**File**: `BACKBONE.API/Controllers/AuthController.cs`

Removed all direct database access and now uses the UnitOfWork to access services:
- `_unitOfWork.UserRepository.GetUserByEmailAsync()` instead of direct SQL query
- `_unitOfWork.UserRepository.GetJwtUserByIdAsync()` instead of direct SQL query
- `_unitOfWork.HrisAuthService.Login()` instead of direct service access
- All other services accessed through UnitOfWork

### 7. Refactored JwtMiddleware

**File**: `BACKBONE.API/Middleware/JwtMiddleware.cs`

Removed direct database access and now uses:
- `_unitOfWork.UserRepository.GetJwtUserByIdAsync()` instead of direct SQL query

## Benefits of These Changes

1. **Clean Architecture Compliance**: Controllers no longer directly access the database
2. **Proper Separation of Concerns**: Database operations are handled in the infrastructure layer
3. **Testability**: Services can be easily mocked for unit testing
4. **Maintainability**: Database logic is centralized in repository classes
5. **Consistency**: All data access follows the same pattern through UnitOfWork

## Code Example - Before and After

### Before (Direct Database Access in Controller):
```csharp
// Direct database access in controller - violates clean architecture
var connectionString = GetConnectionString(1);
IDBHelper _db = new OracleDbHelper(connectionString);
string sql = @"SELECT USER_ID,USER_NAME,USER_MAIL,ROLE_ID,HRIS_USER,USER_PASSWD 
               FROM TNDR_USER T 
               WHERE USER_MAIL=:USER_MAIL AND IS_ACTIVE=1";
var user = _db.QueryFirstOrDefault<TNDR_USER>(sql, new { USER_MAIL = loginDto.EMP_CODE });
```

### After (Using Repository Pattern):
```csharp
// Clean architecture - controller uses repository through UnitOfWork
var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(loginDto.EMP_CODE);
```

## Files Modified

1. `BACKBONE.Application/Interfaces/IUserRepository.cs` - New interface
2. `BACKBONE.Infrastructure/UserRepository.cs` - New implementation
3. `BACKBONE.Application/Interfaces/IUnitOfWork.cs` - Updated interface
4. `BACKBONE.Infrastructure/UnitOfWork.cs` - Updated implementation
5. `BACKBONE.API/Services/ServiceExtension.cs` - Updated service registration
6. `BACKBONE.API/Controllers/AuthController.cs` - Refactored to use UnitOfWork
7. `BACKBONE.API/Middleware/JwtMiddleware.cs` - Refactored to use UnitOfWork

The project now follows clean architecture principles with proper separation of concerns and no direct database access in controllers or middleware.