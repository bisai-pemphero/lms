using LandManagement.Api.DTOs.Withdrawal;
using LandManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WithdrawalsController : ControllerBase
{
    private readonly IWithdrawalService _service;

    public WithdrawalsController(IWithdrawalService service) => _service = service;

    [HttpPost]
    public async Task<ActionResult<WithdrawalResponse>> Withdraw([FromBody] WithdrawalRequest request)
    {
        var user = User.Identity?.Name ?? "Unknown";
        var result = await _service.WithdrawAsync(request, user);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WithdrawalDto>> GetById(int id)
    {
        var withdrawal = await _service.GetByIdAsync(id);
        return withdrawal == null ? NotFound() : Ok(withdrawal);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WithdrawalDto>>> GetAll() 
        => Ok(await _service.GetAllAsync());

    [HttpGet("plot/{plotNo}")]
    public async Task<ActionResult<IEnumerable<WithdrawalDto>>> GetByPlot(string plotNo)
        => Ok(await _service.GetByPlotNoAsync(plotNo));
}
