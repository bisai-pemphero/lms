using LandManagement.Api.DTOs.Receipt;
using LandManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandManagement.Api.Controllers;

/// <summary>
/// Controller for receipt operations based on legacy Receipt.aspx.cs and ViewReceiptPayments.aspx.cs
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReceiptsController : ControllerBase
{
    private readonly IReceiptService _service;
    private readonly ILogger<ReceiptsController> _logger;

    public ReceiptsController(IReceiptService service, ILogger<ReceiptsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get receipt by receipt number
    /// Based on Receipt.aspx.cs LoadReceiptDetails()
    /// </summary>
    /// <param name="receiptNo">Receipt number to lookup</param>
    /// <returns>Receipt details</returns>
    [HttpGet("{receiptNo}")]
    [ProducesResponseType(typeof(ReceiptDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReceiptDto>> GetReceipt(string receiptNo)
    {
        try
        {
            var user = User.Identity?.Name ?? "Unknown";
            _logger.LogInformation("User {User} requesting receipt: {ReceiptNo}", user, receiptNo);

            var receipt = await _service.GetReceiptByReceiptNoAsync(receiptNo);
            
            if (receipt == null)
                return NotFound(new { Success = false, Message = "Receipt not found" });

            return Ok(receipt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting receipt");
            return StatusCode(500, new { Success = false, Message = "An error occurred while retrieving receipt" });
        }
    }

    /// <summary>
    /// Get all receipts for a specific plot
    /// </summary>
    /// <param name="plotNo">Plot number</param>
    /// <returns>List of receipts for the plot</returns>
    [HttpGet("plot/{plotNo}")]
    [ProducesResponseType(typeof(List<ReceiptDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ReceiptDto>>> GetReceiptsByPlot(string plotNo)
    {
        try
        {
            var user = User.Identity?.Name ?? "Unknown";
            _logger.LogInformation("User {User} requesting receipts for plot: {PlotNo}", user, plotNo);

            var receipts = await _service.GetReceiptsByPlotNoAsync(plotNo);
            
            return Ok(receipts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting receipts by plot");
            return StatusCode(500, new { Success = false, Message = "An error occurred while retrieving receipts" });
        }
    }

    /// <summary>
    /// Get all receipts for a specific client
    /// </summary>
    /// <param name="clientNo">Client number</param>
    /// <returns>List of receipts for the client</returns>
    [HttpGet("client/{clientNo}")]
    [ProducesResponseType(typeof(List<ReceiptDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ReceiptDto>>> GetReceiptsByClient(string clientNo)
    {
        try
        {
            var user = User.Identity?.Name ?? "Unknown";
            _logger.LogInformation("User {User} requesting receipts for client: {ClientNo}", user, clientNo);

            var receipts = await _service.GetReceiptsByClientAsync(clientNo);
            
            return Ok(receipts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting receipts by client");
            return StatusCode(500, new { Success = false, Message = "An error occurred while retrieving receipts" });
        }
    }

    /// <summary>
    /// Search receipts with various filters
    /// Based on ViewReceiptPayments.aspx.cs functionality
    /// </summary>
    /// <param name="request">Search criteria</param>
    /// <returns>List of matching receipts</returns>
    [HttpPost("search")]
    [ProducesResponseType(typeof(List<ReceiptDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ReceiptDto>>> SearchReceipts([FromBody] ReceiptQueryRequest request)
    {
        try
        {
            var user = User.Identity?.Name ?? "Unknown";
            _logger.LogInformation("User {User} searching receipts", user);

            var receipts = await _service.SearchReceiptsAsync(request);
            
            return Ok(receipts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching receipts");
            return StatusCode(500, new { Success = false, Message = "An error occurred while searching receipts" });
        }
    }

    /// <summary>
    /// Get receipt payments view - combines receipt with payment history
    /// Based on ViewReceiptPayments.aspx.cs
    /// </summary>
    /// <param name="receiptNo">Receipt number</param>
    /// <returns>Receipt with payment details</returns>
    [HttpGet("view/{receiptNo}")]
    [ProducesResponseType(typeof(ReceiptDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReceiptDto>> ViewReceiptPayments(string receiptNo)
    {
        try
        {
            var user = User.Identity?.Name ?? "Unknown";
            _logger.LogInformation("User {User} viewing receipt payments: {ReceiptNo}", user, receiptNo);

            var receipt = await _service.GetReceiptByReceiptNoAsync(receiptNo);
            
            if (receipt == null)
                return NotFound(new { Success = false, Message = "Receipt not found" });

            return Ok(receipt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error viewing receipt payments");
            return StatusCode(500, new { Success = false, Message = "An error occurred while retrieving receipt details" });
        }
    }
}
