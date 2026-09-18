using System.ComponentModel.DataAnnotations;

namespace LandManagement.Api.DTOs.Auth;

/// <summary>
/// DTO for user login request
/// </summary>
public class LoginRequest
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// DTO for authentication response with JWT token
/// </summary>
public class AuthResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string? Username { get; set; }
    public List<string>? Roles { get; set; }

    public static AuthResponse Fail(string message)
        => new() { Success = false, Message = message };

    public static AuthResponse Ok(string token, string username, List<string> roles, DateTime expiresAt)
        => new() 
        { 
            Success = true, 
            Message = "Login successful",
            Token = token, 
            Username = username, 
            Roles = roles,
            ExpiresAt = expiresAt
        };
}

/// <summary>
/// DTO for refresh token request
/// </summary>
public class RefreshTokenRequest
{
    [Required(ErrorMessage = "Access token is required")]
    public string AccessToken { get; set; } = string.Empty;

    [Required(ErrorMessage = "Refresh token is required")]
    public string RefreshToken { get; set; } = string.Empty;
}
