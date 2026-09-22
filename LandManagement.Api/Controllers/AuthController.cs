using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LandManagement.Api.DTOs.Auth;
using LandManagement.Api.Services;
using LandManagement.Api.Shared.Models;

namespace LandManagement.Api.Controllers;

/// <summary>
/// Controller for authentication operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Authenticate user and return JWT token
    /// </summary>
    /// <param name="request">Login credentials</param>
    /// <returns>JWT token and user information</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Login attempt for username: {Username}", request.Username);

        var result = await _authService.LoginAsync(request.Username, request.Password);

        if (!result.Success)
        {
            _logger.LogWarning("Login failed for username: {Username}", request.Username);
            return BadRequest(ApiResponse<AuthResponse>.Fail(result.Message));
        }

        _logger.LogInformation("Login successful for username: {Username}", request.Username);
        return Ok(ApiResponse.Ok(result, "Login successful"));
    }

    /// <summary>
    /// Refresh access token using refresh token
    /// </summary>
    /// <param name="request">Refresh token request</param>
    /// <returns>New access token</returns>
    [HttpPost("refresh-token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(request.AccessToken, request.RefreshToken);

        if (!result.Success)
        {
            return BadRequest(ApiResponse<AuthResponse>.Fail(result.Message));
        }

        return Ok(ApiResponse.Ok(result, "Token refreshed successfully"));
    }

    /// <summary>
    /// Test endpoint to verify authentication
    /// </summary>
    [HttpGet("test")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public ActionResult<ApiResponse> Test()
    {
        return Ok(ApiResponse.Ok("Authentication successful"));
    }
}
