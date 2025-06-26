# Student Validation HTTP Client

This document explains how to configure and use the HTTP client for student validation in the CoursePlatform application.

## Configuration

### appsettings.json

Add the following configuration to your `appsettings.json` and `appsettings.Development.json` files:

```json
{
  "StudentValidation": {
    "BaseUrl": "https://api.studentvalidation.example.com",
    "ValidationEndpoint": "/api/students/validate",
    "TimeoutSeconds": 30,
    "ApiKey": "your-api-key-here"
  }
}
```

### Configuration Options

- **BaseUrl**: The base URL of the external student validation service
- **ValidationEndpoint**: The specific endpoint for student validation (default: "/api/students/validate")
- **TimeoutSeconds**: HTTP client timeout in seconds (default: 30)
- **ApiKey**: Optional API key for authentication with the external service

## HTTP Client Features

### ✅ Implemented Features

1. **Typed HTTP Client**: Uses `IHttpClientFactory` with typed client pattern
2. **Configuration-based**: Fully configurable through appsettings
3. **Proper Error Handling**: Comprehensive exception handling for various scenarios
4. **Logging**: Structured logging for debugging and monitoring
5. **Timeout Management**: Configurable timeouts to prevent hanging requests
6. **JSON Serialization**: Automatic JSON handling with proper naming policies
7. **Authentication**: Support for API key authentication via headers

### 🔄 Request/Response Format

#### Request Format
The service sends a POST request to the configured endpoint with the following JSON payload:

```json
{
  "name": "John Doe",
  "email": "john.doe@example.com"
}
```

#### Expected Response Format
The external service should respond with:

**Success Response (200 OK):**
```json
{
  "isValid": true
}
```

**Validation Failure Response (200 OK):**
```json
{
  "isValid": false,
  "errorMessage": "Student validation failed",
  "validationErrors": [
    "Email domain is not allowed",
    "Student already exists in external system"
  ]
}
```

### 🚨 Error Handling

The HTTP client handles the following error scenarios:

1. **Network Errors**: `HttpRequestException` - Returns user-friendly error message
2. **Timeouts**: `TaskCanceledException` - Handles both request and operation timeouts
3. **JSON Errors**: `JsonException` - Handles malformed response data
4. **HTTP Errors**: Non-success status codes - Logs full error details
5. **Unexpected Errors**: Generic exception handling with logging

### 📝 Usage Example

The service is automatically injected into command handlers:

```csharp
public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, StudentResponseDTO>
{
    private readonly IStudentValidationService _validationService;

    public CreateStudentCommandHandler(IStudentValidationService validationService)
    {
        _validationService = validationService;
    }

    public async Task<StudentResponseDTO> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validationService.ValidateStudentAsync(request.Student, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.ErrorMessage ?? "Student validation failed", 
                validationResult.ValidationErrors);
        }

        // Continue with student creation...
    }
}
```

### 🧪 Testing

For testing purposes, you can use the mock implementation:

```csharp
// In your test setup
services.AddScoped<IStudentValidationService, MockStudentValidationService>();

// Or create a custom mock with specific behavior
var mockService = new MockStudentValidationService(
    invalidEmails: new List<string> { "invalid@test.com" },
    shouldThrowException: false,
    simulatedDelay: TimeSpan.FromSeconds(1)
);
```

### 🔧 Advanced Configuration

For production environments, consider adding:

1. **Retry Policies**: Using Polly for resilient HTTP calls
2. **Circuit Breaker**: To handle cascading failures
3. **Health Checks**: Monitor external service availability
4. **Metrics**: Track response times and success rates

Example with Polly (requires additional NuGet package):

```csharp
services.AddHttpClient<IStudentValidationService, StudentValidationService>()
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy());
```

### 📊 Monitoring and Logging

The service provides structured logging with the following log levels:

- **Information**: Successful validations and request/response details
- **Warning**: Retry attempts and non-critical issues
- **Error**: HTTP errors, timeouts, and exceptions

Log examples:
```
[Information] Starting validation for student with email: john.doe@example.com
[Information] Validation completed for student john.doe@example.com. Result: True
[Error] HTTP error occurred while validating student john.doe@example.com
```

### 🔐 Security Considerations

1. **API Keys**: Store API keys in secure configuration (Azure Key Vault, etc.)
2. **HTTPS**: Always use HTTPS for external service communication
3. **Input Validation**: The external service should validate all input data
4. **Rate Limiting**: Be aware of external service rate limits
5. **Data Privacy**: Ensure student data is handled according to privacy regulations
