using LandManagement.Api.DTOs.Report;
using LandManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandManagement.Api.Controllers;

/// <summary>
/// Controller for generating reports based on legacy SalesReport.aspx.cs and Reminder.aspx.cs
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportService _service;
    private readonly ILogger<ReportsController> _logger;

    public ReportsController(IReportService service, ILogger<ReportsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Generate sales report with statistics and records
    /// Based on SalesReport.aspx.cs LoadStats() and SalesReport() methods
    /// </summary>
    /// <param name="request">Report parameters including location, start date, end date</param>
    /// <returns>Sales report with statistics, records, and overdue plots</returns>
    [HttpPost("sales")]
    [ProducesResponseType(typeof(SalesReportResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SalesReportResponse>> GetSalesReport([FromBody] SalesReportRequest request)
    {
        try
        {
            // Validate required parameters
            if (string.IsNullOrEmpty(request.Location))
            {
                return BadRequest(new ApiResponse { Success = false, Message = "Location is required" });
            }

            if (request.StartDate == default || request.EndDate == default)
            {
                return BadRequest(new ApiResponse { Success = false, Message = "Start date and end date are required" });
            }

            if (request.StartDate > request.EndDate)
            {
                return BadRequest(new ApiResponse { Success = false, Message = "Start date must be before end date" });
            }

            var user = User.Identity?.Name ?? "Unknown";
            _logger.LogInformation("User {User} requesting sales report for location: {Location}, from: {StartDate} to: {EndDate}", 
                user, request.Location, request.StartDate, request.EndDate);

            var report = await _service.GetSalesReportAsync(request);
            
            return Ok(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating sales report");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred while generating the report" });
        }
    }

    /// <summary>
    /// Get payment reminders for customers with balances and offer period > 12 months
    /// Based on Reminder.aspx.cs LoadReminderLogs() method
    /// </summary>
    /// <returns>List of customers requiring payment reminders</returns>
    [HttpGet("reminders")]
    [ProducesResponseType(typeof(ReminderResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ReminderResponse>> GetReminders()
    {
        try
        {
            var user = User.Identity?.Name ?? "Unknown";
            _logger.LogInformation("User {User} requesting payment reminders", user);

            var reminders = await _service.GetRemindersAsync();
            
            return Ok(reminders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting reminders");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred while retrieving reminders" });
        }
    }

    /// <summary>
    /// Send SMS reminders to customers with outstanding balances
    /// Based on Reminder.aspx.cs SMS integration logic
    /// </summary>
    /// <param name="request">Reminder request with optional phone numbers</param>
    /// <returns>Result of reminder sending operation</returns>
    [HttpPost("reminders/send")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> SendReminders([FromBody] SendReminderRequest? request = null)
    {
        try
        {
            var user = User.Identity?.Name ?? "Unknown";
            _logger.LogInformation("User {User} initiating reminder sending", user);

            // Get all reminders
            var reminders = await _service.GetRemindersAsync();
            
            if (reminders.Records.Count == 0)
            {
                return Ok(new ApiResponse { Success = true, Message = "No reminders to send" });
            }

            // TODO: Implement SMS sending logic based on legacy SendText.aspx.cs
            // For now, return success with count
            _logger.LogInformation("Prepared {Count} reminders for sending", reminders.Records.Count);

            return Ok(new ApiResponse 
            { 
                Success = true, 
                Message = $"Prepared {reminders.Records.Count} reminders for sending",
                Data = new { TotalReminders = reminders.Records.Count }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending reminders");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred while sending reminders" });
        }
    }
}

/// <summary>
/// Request model for sending reminders
/// </summary>
public class SendReminderRequest
{
    public List<string>? PhoneNumbers { get; set; }
    public string? CustomMessage { get; set; }
}

/// <summary>
/// Generic API response model
/// </summary>
public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }
}
