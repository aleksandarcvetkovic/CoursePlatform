# Clean Architecture - Proper Layer Separation with Class Libraries

## Current Issues with Single Project Structure

Your current structure has all layers in one project:
```
CoursePlatform/
├── Domain/
├── Application/
├── Infrastructure/
└── Presentation/
```

This allows for architectural violations because there's no compile-time enforcement of dependency rules.

## Recommended Project Structure

```
CoursePlatform.sln
├── src/
│   ├── CoursePlatform.Domain/              (Class Library)
│   ├── CoursePlatform.Application/         (Class Library)
│   ├── CoursePlatform.Infrastructure/      (Class Library)
│   ├── CoursePlatform.Presentation.Api/    (Web API Project)
│   └── CoursePlatform.Presentation.Web/    (MVC/Blazor Project - Optional)
├── tests/
│   ├── CoursePlatform.Domain.Tests/
│   ├── CoursePlatform.Application.Tests/
│   ├── CoursePlatform.Infrastructure.Tests/
│   └── CoursePlatform.Integration.Tests/
└── docs/
```

## Benefits of Separate Class Libraries

### 1. **Compile-Time Dependency Enforcement**
```csharp
// This would cause a compile error if Domain references Infrastructure
// Domain project cannot reference Infrastructure project
```

### 2. **Clear Boundaries**
- Each layer has its own assembly
- Dependencies are explicit via project references
- Easier to see architectural violations

### 3. **Deployment Flexibility**
- Can deploy different layers to different services
- Better support for microservices architecture
- Easier to create separate deployment packages

### 4. **Team Collaboration**
- Different teams can work on different layers
- Clearer ownership and responsibilities
- Better merge conflict resolution

## Dependency Rules (Project References)

### ✅ Allowed Dependencies:
```
API Project → Application → Domain
Infrastructure → Application → Domain
Application.Tests → Application → Domain
```

### ❌ Forbidden Dependencies:
```
Domain → Application (NEVER)
Domain → Infrastructure (NEVER)
Application → Infrastructure (NEVER - except through interfaces)
```

## Migration Steps

### Step 1: Create New Class Library Projects
```bash
# Create solution structure
mkdir src tests docs

# Create Domain layer
dotnet new classlib -n CoursePlatform.Domain -o src/CoursePlatform.Domain

# Create Application layer  
dotnet new classlib -n CoursePlatform.Application -o src/CoursePlatform.Application

# Create Infrastructure layer
dotnet new classlib -n CoursePlatform.Infrastructure -o src/CoursePlatform.Infrastructure

# Create API layer
dotnet new webapi -n CoursePlatform.Api -o src/CoursePlatform.Api

# Add to solution
dotnet sln add src/CoursePlatform.Domain/CoursePlatform.Domain.csproj
dotnet sln add src/CoursePlatform.Application/CoursePlatform.Application.csproj  
dotnet sln add src/CoursePlatform.Infrastructure/CoursePlatform.Infrastructure.csproj
dotnet sln add src/CoursePlatform.Api/CoursePlatform.Api.csproj
```

### Step 2: Set Up Project References
```bash
# Application references Domain
dotnet add src/CoursePlatform.Application/CoursePlatform.Application.csproj reference src/CoursePlatform.Domain/CoursePlatform.Domain.csproj

# Infrastructure references Application and Domain
dotnet add src/CoursePlatform.Infrastructure/CoursePlatform.Infrastructure.csproj reference src/CoursePlatform.Application/CoursePlatform.Application.csproj
dotnet add src/CoursePlatform.Infrastructure/CoursePlatform.Infrastructure.csproj reference src/CoursePlatform.Domain/CoursePlatform.Domain.csproj

# API references Application and Infrastructure
dotnet add src/CoursePlatform.Api/CoursePlatform.Api.csproj reference src/CoursePlatform.Application/CoursePlatform.Application.csproj
dotnet add src/CoursePlatform.Api/CoursePlatform.Api.csproj reference src/CoursePlatform.Infrastructure/CoursePlatform.Infrastructure.csproj
```

### Step 3: Move Files to Appropriate Projects

#### Domain Project (`CoursePlatform.Domain`)
- `Domain/Entities/`
- `Domain/Repositories/` (interfaces only)
- `Domain/Services/` (interfaces and implementations)
- `Domain/Events/`
- `Domain/Common/`
- `Domain/Exceptions/`

#### Application Project (`CoursePlatform.Application`)
- `Application/Features/`
- `Application/DTOs/`
- `Application/Common/Behaviors/`
- `Application/Common/Interfaces/`
- `Application/Common/Mappings/`
- `Application/Common/Exceptions/`
- `Application/DependencyInjection.cs`

#### Infrastructure Project (`CoursePlatform.Infrastructure`)
- `Infrastructure/Persistence/`
- `Infrastructure/Repositories/` (implementations)
- `Infrastructure/Services/` (external service implementations)
- `Infrastructure/DependencyInjection.cs`

#### API Project (`CoursePlatform.Api`)
- `Presentation/Endpoints/`
- `Program.cs`
- `appsettings.json`
- Controllers (if using)

## Example Project Files

### Domain Project (`CoursePlatform.Domain.csproj`)
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <!-- No external dependencies - Domain is the center -->
</Project>
```

### Application Project (`CoursePlatform.Application.csproj`)
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="MediatR" Version="12.4.1" />
    <PackageReference Include="FluentValidation" Version="11.11.0" />
    <PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="11.11.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\CoursePlatform.Domain\CoursePlatform.Domain.csproj" />
  </ItemGroup>
</Project>
```

### Infrastructure Project (`CoursePlatform.Infrastructure.csproj`)
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.6" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.6" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\CoursePlatform.Application\CoursePlatform.Application.csproj" />
    <ProjectReference Include="..\CoursePlatform.Domain\CoursePlatform.Domain.csproj" />
  </ItemGroup>
</Project>
```

### API Project (`CoursePlatform.Api.csproj`)
```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.6" />
    <PackageReference Include="NSwag.AspNetCore" Version="14.4.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\CoursePlatform.Application\CoursePlatform.Application.csproj" />
    <ProjectReference Include="..\CoursePlatform.Infrastructure\CoursePlatform.Infrastructure.csproj" />
  </ItemGroup>
</Project>
```

## Namespace Conventions

```csharp
// Domain
namespace CoursePlatform.Domain.Entities;
namespace CoursePlatform.Domain.Repositories;
namespace CoursePlatform.Domain.Services;

// Application  
namespace CoursePlatform.Application.Features.Students.Commands;
namespace CoursePlatform.Application.Features.Students.Queries;
namespace CoursePlatform.Application.Common.Interfaces;

// Infrastructure
namespace CoursePlatform.Infrastructure.Persistence;
namespace CoursePlatform.Infrastructure.Repositories;

// API
namespace CoursePlatform.Api.Endpoints;
namespace CoursePlatform.Api.Controllers;
```

## Testing Structure

```bash
# Create test projects
dotnet new xunit -n CoursePlatform.Domain.Tests -o tests/CoursePlatform.Domain.Tests
dotnet new xunit -n CoursePlatform.Application.Tests -o tests/CoursePlatform.Application.Tests  
dotnet new xunit -n CoursePlatform.Infrastructure.Tests -o tests/CoursePlatform.Infrastructure.Tests
dotnet new xunit -n CoursePlatform.Integration.Tests -o tests/CoursePlatform.Integration.Tests
```

## Migration Strategy

1. **Create new solution structure** with separate projects
2. **Move files gradually** by layer (start with Domain)
3. **Update namespaces** to match new project structure
4. **Fix project references** one layer at a time
5. **Update tests** to reference correct projects
6. **Verify compilation** after each layer migration

## Immediate Benefits

- ✅ **Compile-time safety** - Cannot accidentally reference wrong layers
- ✅ **Clear dependencies** - Easy to see what references what
- ✅ **Better organization** - Each layer is self-contained
- ✅ **Team collaboration** - Different teams can own different projects
- ✅ **Deployment flexibility** - Can deploy layers separately if needed
- ✅ **Testing isolation** - Can test each layer independently

This structure will make your Clean Architecture implementation much more robust and prevent architectural violations at compile time.
