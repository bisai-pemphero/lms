using LandManagement.Api.DTOs.Creditor;
using LandManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CreditorsController : ControllerBase
{
    private readonly ICreditorService _service;

    public CreditorsController(ICreditorService service) => _service = service;

    [HttpPost]
    public async Task<ActionResult<CreditorResponse>> Create([FromBody] CreditorRequest request)
    {
        var user = User.Identity?.Name ?? "Unknown";
        var result = await _service.CreateAsync(request, user);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CreditorDto>> GetById(int id)
    {
        var creditor = await _service.GetByIdAsync(id);
        return creditor == null ? NotFound() : Ok(creditor);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CreditorDto>>> GetAll() 
        => Ok(await _service.GetAllAsync());
}
