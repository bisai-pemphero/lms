using LandManagement.Api.DTOs.ChangeOfOwnership;
using LandManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChangeOfOwnershipsController : ControllerBase
{
    private readonly IChangeOfOwnershipService _service;

    public ChangeOfOwnershipsController(IChangeOfOwnershipService service) => _service = service;

    [HttpPost]
    public async Task<ActionResult<ChangeOfOwnershipResponse>> Transfer([FromBody] ChangeOfOwnershipRequest request)
    {
        var user = User.Identity?.Name ?? "Unknown";
        var result = await _service.TransferOwnershipAsync(request, user);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ChangeOfOwnershipDto>> GetById(int id)
    {
        var record = await _service.GetByIdAsync(id);
        return record == null ? NotFound() : Ok(record);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ChangeOfOwnershipDto>>> GetAll() 
        => Ok(await _service.GetAllAsync());

    [HttpGet("plot/{plotNo}")]
    public async Task<ActionResult<IEnumerable<ChangeOfOwnershipDto>>> GetByPlot(string plotNo)
        => Ok(await _service.GetByPlotNoAsync(plotNo));
}
