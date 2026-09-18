using LandManagement.Api.DTOs.Dashboard;
using LandManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService service) => _service = service;

    [HttpGet("summary")]
    public async Task<ActionResult<DashboardSummaryDto>> GetSummary()
        => Ok(await _service.GetSummaryAsync());

    [HttpGet("site-performance")]
    public async Task<ActionResult<IEnumerable<SitePerformanceDto>>> GetSitePerformance()
        => Ok(await _service.GetSitePerformanceAsync());

    [HttpGet("recent-transactions")]
    public async Task<ActionResult<IEnumerable<RecentTransactionDto>>> GetRecentTransactions([FromQuery] int count = 10)
        => Ok(await _service.GetRecentTransactionsAsync(count));
}
