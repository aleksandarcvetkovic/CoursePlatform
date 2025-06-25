using CoursePlatform.Infrastructure.Common.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CoursePlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DebugController : ControllerBase
{
    private readonly DatabaseOptions _databaseOptions;

    public DebugController(IOptions<DatabaseOptions> databaseOptions)
    {
        _databaseOptions = databaseOptions.Value;
    }

    [HttpGet("database-config")]
    public IActionResult GetDatabaseConfig()
    {
        return Ok(new
        {
            ConnectionString = _databaseOptions.ConnectionString,
            MaxRetryCount = _databaseOptions.MaxRetryCount,
            MaxRetryDelaySeconds = _databaseOptions.MaxRetryDelaySeconds,
            EnableRetryOnFailure = _databaseOptions.EnableRetryOnFailure,
            ConnectionStringLength = _databaseOptions.ConnectionString?.Length ?? 0,
            IsConnectionStringEmpty = string.IsNullOrEmpty(_databaseOptions.ConnectionString)
        });
    }
}
