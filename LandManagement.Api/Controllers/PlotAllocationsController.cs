using LandManagement.Api.DTOs.PlotAllocation;
using LandManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlotAllocationsController : ControllerBase
{
    private readonly IPlotAllocationService _service;
    private readonly ILogger<PlotAllocationsController> _logger;

    public PlotAllocationsController(IPlotAllocationService service, ILogger<PlotAllocationsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(PlotAllocationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PlotAllocationResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PlotAllocationResponse>> AllocatePlot([FromBody] PlotAllocationRequest request)
    {
        var user = User.Identity?.Name ?? "Unknown";
        var result = await _service.AllocatePlotAsync(request, user);
        
        if (!result.Success)
            return BadRequest(result);
        
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PlotAllocationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlotAllocationDto>> GetById(int id)
    {
        var allocation = await _service.GetByIdAsync(id);
        if (allocation == null)
            return NotFound();
        
        return Ok(allocation);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PlotAllocationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PlotAllocationDto>>> GetAll()
    {
        var allocations = await _service.GetAllAsync();
        return Ok(allocations);
    }

    [HttpGet("client/{clientNo}")]
    [ProducesResponseType(typeof(IEnumerable<PlotAllocationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PlotAllocationDto>>> GetByClient(string clientNo)
    {
        var allocations = await _service.GetByClientNoAsync(clientNo);
        return Ok(allocations);
    }

    [HttpGet("plot/{plotNo}")]
    [ProducesResponseType(typeof(IEnumerable<PlotAllocationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PlotAllocationDto>>> GetByPlot(string plotNo)
    {
        var allocations = await _service.GetByPlotNoAsync(plotNo);
        return Ok(allocations);
    }
}
