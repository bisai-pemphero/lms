namespace LandManagement.Api.Exceptions;

/// <summary>
/// Base exception for application-specific errors
/// </summary>
public class AppException : Exception
{
    public int StatusCode { get; set; } = 400;

    public AppException(string message) : base(message)
    {
    }

    public AppException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    public AppException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception for resource not found errors
/// </summary>
public class NotFoundException : AppException
{
    public NotFoundException(string message) : base(message, 404)
    {
    }
}

/// <summary>
/// Exception for validation errors
/// </summary>
public class ValidationException : AppException
{
    public List<string> Errors { get; set; } = new();

    public ValidationException(string message) : base(message)
    {
    }

    public ValidationException(List<string> errors) : base("Validation failed")
    {
        Errors = errors;
    }
}

/// <summary>
/// Exception for unauthorized access
/// </summary>
public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message) : base(message, 401)
    {
    }
}

/// <summary>
/// Exception for forbidden access
/// </summary>
public class ForbiddenException : AppException
{
    public ForbiddenException(string message) : base(message, 403)
    {
    }
}

/// <summary>
/// Exception for conflict errors (e.g., duplicate records)
/// </summary>
public class ConflictException : AppException
{
    public ConflictException(string message) : base(message, 409)
    {
    }
}
