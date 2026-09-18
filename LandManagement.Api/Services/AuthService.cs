using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using LandManagement.Api.Entities;
using LandManagement.Api.DTOs.Auth;
using Microsoft.EntityFrameworkCore;
using LandManagement.Api.Data;
using LandManagement.Api.Config;

namespace LandManagement.Api.Services;

/// <summary>
/// Service for handling authentication operations
/// </summary>
public interface IAuthService
{
    Task<AuthResponse> LoginAsync(string username, string password);
    Task<AuthResponse> RefreshTokenAsync(string accessToken, string refreshToken);
    Task<bool> ValidateUserAsync(string username, string password);
}

/// <summary>
/// Implementation of authentication service
/// </summary>
public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        ApplicationDbContext context,
        JwtSettings jwtSettings,
        ILogger<AuthService> logger)
    {
        _context = context;
        _jwtSettings = jwtSettings;
        _logger = logger;
    }

    public async Task<AuthResponse> LoginAsync(string username, string password)
    {
        try
        {
            // Find user by username (checking both Users and CUsers tables)
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                // Try CUsers table for customer users
                var cUser = await _context.CUsers
                    .FirstOrDefaultAsync(u => u.Username == username);
                
                if (cUser == null)
                {
                    return AuthResponse.Fail("Invalid username or password");
                }

                // Validate CUser password (legacy TripleDES or plain text)
                if (!ValidateCUserPassword(cUser, password))
                {
                    return AuthResponse.Fail("Invalid username or password");
                }

                return GenerateTokenForCUser(cUser);
            }

            // Validate User password (legacy TripleDES or plain text)
            if (!ValidateUserPassword(user, password))
            {
                return AuthResponse.Fail("Invalid username or password");
            }

            return GenerateTokenForUser(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for username: {Username}", username);
            return AuthResponse.Fail("An error occurred during authentication");
        }
    }

    public async Task<AuthResponse> RefreshTokenAsync(string accessToken, string refreshToken)
    {
        // TODO: Implement refresh token logic when refresh tokens are stored
        return await Task.FromResult(AuthResponse.Fail("Refresh tokens not yet implemented"));
    }

    public async Task<bool> ValidateUserAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        
        if (user == null)
        {
            var cUser = await _context.CUsers.FirstOrDefaultAsync(u => u.Username == username);
            return cUser != null && ValidateCUserPassword(cUser, password);
        }

        return ValidateUserPassword(user, password);
    }

    /// <summary>
    /// Validates password against legacy TripleDES encryption or plain text
    /// </summary>
    private bool ValidateUserPassword(User user, string providedPassword)
    {
        if (string.IsNullOrEmpty(user.Password))
            return false;

        // Check if password is already hashed (BCrypt format starts with $2a$)
        if (user.Password.StartsWith("$2a$"))
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, user.Password);
        }

        // Legacy: Try TripleDES decryption
        try
        {
            var decryptedPassword = DecryptLegacyPassword(user.Password);
            if (decryptedPassword == providedPassword)
                return true;
        }
        catch
        {
            // Decryption failed, continue to plain text check
        }

        // Plain text comparison (insecure, but supports legacy data)
        return user.Password == providedPassword;
    }

    /// <summary>
    /// Validates CUser password
    /// </summary>
    private bool ValidateCUserPassword(Entities.CUser cUser, string providedPassword)
    {
        if (string.IsNullOrEmpty(cUser.Password))
            return false;

        // Legacy passwords may be plain text or encrypted
        return cUser.Password == providedPassword;
    }

    /// <summary>
    /// Decrypts legacy TripleDES encrypted password
    /// </summary>
    private string DecryptLegacyPassword(string encryptedPassword)
    {
        // Legacy key from the original system
        var key = "LandManagement20"; // Must be 16 bytes for TripleDES
        
        using var tdes = System.Security.Cryptography.TripleDES.Create();
        tdes.Key = Encoding.UTF8.GetBytes(key.PadRight(24).Substring(0, 24));
        tdes.IV = new byte[tdes.BlockSize / 8];
        tdes.Mode = System.Security.Cryptography.CipherMode.ECB;
        tdes.Padding = System.Security.Cryptography.PaddingMode.None;

        var encryptedBytes = Convert.FromBase64String(encryptedPassword);
        var decryptor = tdes.CreateDecryptor();
        var decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
        
        return Encoding.UTF8.GetString(decryptedBytes).TrimEnd('\0');
    }

    /// <summary>
    /// Generates JWT token for User
    /// </summary>
    private AuthResponse GenerateTokenForUser(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new("UserId", user.UserId.ToString())
        };

        if (user.Role != null)
        {
            claims.Add(new Claim(ClaimTypes.Role, user.Role.RoleName));
            claims.Add(new Claim("RoleId", user.RoleId.ToString()));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.WriteToken(token);

        return new AuthResponse
        {
            Success = true,
            Message = "Login successful",
            Token = jwtToken,
            ExpiresAt = expiresAt,
            Username = user.Username,
            Roles = user.Role != null ? new List<string> { user.Role.RoleName } : new List<string>()
        };
    }

    /// <summary>
    /// Generates JWT token for CUser
    /// </summary>
    private AuthResponse GenerateTokenForCUser(Entities.CUser cUser)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, cUser.Username),
            new(ClaimTypes.NameIdentifier, cUser.Id.ToString()),
            new("CUserId", cUser.Id.ToString()),
            new("UserType", "Customer")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.WriteToken(token);

        return new AuthResponse
        {
            Success = true,
            Message = "Login successful",
            Token = jwtToken,
            ExpiresAt = expiresAt,
            Username = cUser.Username,
            Roles = new List<string> { "Customer" }
        };
    }
}

/// <summary>
/// Extension methods for AuthResponse
/// </summary>
public static class AuthResponseExtensions
{
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
