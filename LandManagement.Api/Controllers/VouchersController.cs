using LandManagement.Api.DTOs.Voucher;
using LandManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandManagement.Api.Controllers;

/// <summary>
/// Controller for payment voucher operations based on legacy Vouchers.aspx.cs, VoucherPayment.aspx.cs, and VoucherCreditor.aspx.cs
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VouchersController : ControllerBase
{
    private readonly IVoucherService _service;
    private readonly ILogger<VouchersController> _logger;

    public VouchersController(IVoucherService service, ILogger<VouchersController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all pending vouchers (regular or creditor)
    /// Based on Vouchers.aspx.cs LoadCustomerDetails() and VoucherCreditor.aspx.cs
    /// </summary>
    /// <param name="isCreditor">True for creditor vouchers, false for regular vouchers</param>
    /// <returns>List of pending vouchers</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<VoucherDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<VoucherDto>>> GetPendingVouchers([FromQuery] bool isCreditor = false)
    {
        try
        {
            var user = User.Identity?.Name ?? "Unknown";
            _logger.LogInformation("User {User} requesting pending {VoucherType} vouchers", 
                user, isCreditor ? "creditor" : "regular");

            var vouchers = await _service.GetPendingVouchersAsync(isCreditor);
            
            return Ok(vouchers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting pending vouchers");
            return StatusCode(500, new { Success = false, Message = "An error occurred while retrieving vouchers" });
        }
    }

    /// <summary>
    /// Get voucher by ID with full details
    /// Based on VoucherPayment.aspx.cs LoadVoucherDetails()
    /// </summary>
    /// <param name="id">Voucher ID</param>
    /// <param name="isCreditor">True for creditor vouchers, false for regular vouchers</param>
    /// <returns>Voucher details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(VoucherDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VoucherDto>> GetVoucherById(int id, [FromQuery] bool isCreditor = false)
    {
        try
        {
            var user = User.Identity?.Name ?? "Unknown";
            _logger.LogInformation("User {User} requesting voucher {VoucherId}", user, id);

            var voucher = await _service.GetVoucherByIdAsync(id, isCreditor);
            
            if (voucher == null)
                return NotFound(new { Success = false, Message = "Voucher not found" });

            return Ok(voucher);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting voucher by ID");
            return StatusCode(500, new { Success = false, Message = "An error occurred while retrieving voucher" });
        }
    }

    /// <summary>
    /// Create a new payment voucher
    /// Based on voucher creation logic in legacy code
    /// </summary>
    /// <param name="request">Voucher creation request</param>
    /// <returns>Created voucher</returns>
    [HttpPost]
    [ProducesResponseType(typeof(VoucherDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VoucherDto>> CreateVoucher([FromBody] CreateVoucherRequest request)
    {
        try
        {
            // Validate request
            if (string.IsNullOrEmpty(request.PayeeName))
            {
                return BadRequest(new { Success = false, Message = "Payee name is required" });
            }

            if (request.Amount <= 0)
            {
                return BadRequest(new { Success = false, Message = "Amount must be greater than zero" });
            }

            var user = User.Identity?.Name ?? "Unknown";
            _logger.LogInformation("User {User} creating {VoucherType} voucher for {Payee}", 
                user, request.IsCreditor ? "creditor" : "regular", request.PayeeName);

            var voucher = await _service.CreateVoucherAsync(request, user);
            
            return CreatedAtAction(nameof(GetVoucherById), new { id = voucher.PaymentVoucherId, isCreditor = request.IsCreditor }, voucher);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating voucher");
            return StatusCode(500, new { Success = false, Message = "An error occurred while creating voucher" });
        }
    }

    /// <summary>
    /// Approve a payment voucher
    /// Based on VoucherPayment.aspx.cs btnSave_Click()
    /// </summary>
    /// <param name="id">Voucher ID to approve</param>
    /// <param name="isCreditor">True for creditor vouchers, false for regular vouchers</param>
    /// <returns>Updated voucher</returns>
    [HttpPost("{id}/approve")]
    [ProducesResponseType(typeof(VoucherDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VoucherDto>> ApproveVoucher(int id, [FromQuery] bool isCreditor = false)
    {
        try
        {
            var user = User.Identity?.Name ?? "Unknown";
            _logger.LogInformation("User {User} approving {VoucherType} voucher {VoucherId}", 
                user, isCreditor ? "creditor" : "regular", id);

            var voucher = await _service.ApproveVoucherAsync(id, user, isCreditor);
            
            if (voucher == null)
                return NotFound(new { Success = false, Message = "Voucher not found" });

            return Ok(voucher);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving voucher");
            return StatusCode(500, new { Success = false, Message = "An error occurred while approving voucher" });
        }
    }

    /// <summary>
    /// Get all vouchers (including approved ones) with optional filters
    /// </summary>
    /// <param name="status">Filter by status (Pending, Approved)</param>
    /// <param name="isCreditor">True for creditor vouchers, false for regular vouchers</param>
    /// <param name="startDate">Filter by created date from</param>
    /// <param name="endDate">Filter by created date to</param>
    /// <returns>List of vouchers matching criteria</returns>
    [HttpGet("all")]
    [ProducesResponseType(typeof(List<VoucherDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<VoucherDto>>> GetAllVouchers(
        [FromQuery] string? status = null,
        [FromQuery] bool isCreditor = false,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var user = User.Identity?.Name ?? "Unknown";
            _logger.LogInformation("User {User} requesting filtered vouchers", user);

            // For now, return pending vouchers - can be extended based on requirements
            var vouchers = await _service.GetPendingVouchersAsync(isCreditor);
            
            // Apply status filter
            if (!string.IsNullOrEmpty(status))
            {
                vouchers = vouchers.Where(v => v.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Apply date range filter
            if (startDate.HasValue)
            {
                vouchers = vouchers.Where(v => v.CreatedDate >= startDate.Value).ToList();
            }
            
            if (endDate.HasValue)
            {
                vouchers = vouchers.Where(v => v.CreatedDate <= endDate.Value).ToList();
            }

            return Ok(vouchers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all vouchers");
            return StatusCode(500, new { Success = false, Message = "An error occurred while retrieving vouchers" });
        }
    }
}
