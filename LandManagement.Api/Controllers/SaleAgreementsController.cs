using LandManagement.Api.DTOs.SaleAgreement;
using LandManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SaleAgreementsController : ControllerBase
{
    private readonly ISaleAgreementService _service;

    public SaleAgreementsController(ISaleAgreementService service) => _service = service;

    [HttpPost]
    public async Task<ActionResult<SaleAgreementResponse>> Create([FromBody] SaleAgreementRequest request)
    {
        var user = User.Identity?.Name ?? "Unknown";
        var result = await _service.CreateAsync(request, user);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SaleAgreementDto>> GetById(int id)
    {
        var agreement = await _service.GetByIdAsync(id);
        return agreement == null ? NotFound() : Ok(agreement);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SaleAgreementDto>>> GetAll() 
        => Ok(await _service.GetAllAsync());

    [HttpGet("client/{clientNo}")]
    public async Task<ActionResult<IEnumerable<SaleAgreementDto>>> GetByClient(string clientNo)
        => Ok(await _service.GetByClientNoAsync(clientNo));
}
