using LandManagement.Api.DTOs.PlotPayment;
using LandManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlotPaymentsController : ControllerBase
{
    private readonly IPlotPaymentService _service;
    private readonly ILogger<PlotPaymentsController> _logger;

    public PlotPaymentsController(IPlotPaymentService service, ILogger<PlotPaymentsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(PlotPaymentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PlotPaymentResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PlotPaymentResponse>> RecordPayment([FromBody] PlotPaymentRequest request)
    {
        var user = User.Identity?.Name ?? "Unknown";
        var result = await _service.RecordPaymentAsync(request, user);
        
        if (!result.Success)
            return BadRequest(result);
        
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PlotPaymentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlotPaymentDto>> GetById(int id)
    {
        var payment = await _service.GetByIdAsync(id);
        if (payment == null)
            return NotFound();
        
        return Ok(payment);
    }

    [HttpGet("plot/{plotNo}")]
    [ProducesResponseType(typeof(IEnumerable<PlotPaymentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PlotPaymentDto>>> GetByPlot(string plotNo)
    {
        var payments = await _service.GetByPlotNoAsync(plotNo);
        return Ok(payments);
    }

    [HttpGet("client/{clientNo}")]
    [ProducesResponseType(typeof(IEnumerable<PlotPaymentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PlotPaymentDto>>> GetByClient(string clientNo)
    {
        var payments = await _service.GetByClientNoAsync(clientNo);
        return Ok(payments);
    }
}
