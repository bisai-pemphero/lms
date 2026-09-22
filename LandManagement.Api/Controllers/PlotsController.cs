using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LandManagement.Api.DTOs.Plot;
using LandManagement.Api.Entities;
using LandManagement.Api.Services;
using LandManagement.Api.Shared.Models;

namespace LandManagement.Api.Controllers;

/// <summary>
/// Controller for plot operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlotsController : ControllerBase
{
    private readonly IPlotService _plotService;
    private readonly ILogger<PlotsController> _logger;

    public PlotsController(IPlotService plotService, ILogger<PlotsController> logger)
    {
        _plotService = plotService;
        _logger = logger;
    }

    /// <summary>
    /// Get all plots with optional site and status filters
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<PlotResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<PlotResponse>>>> GetAll(
        [FromQuery] string? siteCode = null, 
        [FromQuery] string? status = null,
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10)
    {
        var plots = await _plotService.SearchAsync("", siteCode, status, page, pageSize);
        
        var response = plots.Select(p => new PlotResponse
        {
            PlotNo = p.PlotNo,
            SiteNo = p.SiteNo ?? "",
            SiteName = p.Site?.SiteName,
            Block = p.Block,
            Area = p.Size,
            Value = p.Value,
            NormalPrice = p.NormalPrice,
            PromotionPrice = p.PromotionPrice,
            LandTitle = p.LandTitle,
            Description = p.Description,
            Status = p.Status ?? "",
            DateCreated = p.DateCreated
        }).ToList();

        return Ok(ApiResponse.Ok(response));
    }

    /// <summary>
    /// Get plot by plot number
    /// </summary>
    [HttpGet("{plotNo}")]
    [ProducesResponseType(typeof(ApiResponse<PlotResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PlotResponse>>> GetByPlotNo(string plotNo)
    {
        var plot = await _plotService.GetByPlotNoAsync(plotNo);

        if (plot == null)
            return NotFound(ApiResponse<PlotResponse>.Fail($"Plot with number {plotNo} not found"));

        var response = new PlotResponse
        {
            PlotNo = plot.PlotNo,
            SiteNo = plot.SiteNo ?? "",
            SiteName = plot.Site?.SiteName,
            Block = plot.Block,
            Area = plot.Size,
            Value = plot.Value,
            NormalPrice = plot.NormalPrice,
            PromotionPrice = plot.PromotionPrice,
            LandTitle = plot.LandTitle,
            Description = plot.Description,
            Status = plot.Status ?? "",
            DateCreated = plot.DateCreated
        };

        return Ok(ApiResponse.Ok(response));
    }

    /// <summary>
    /// Create a new plot
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PlotResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<PlotResponse>>> Create([FromBody] PlotCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<PlotResponse>.Fail("Invalid plot data", GetValidationErrors()));

        var plot = new Plot
        {
            PlotNo = request.PlotNo,
            SiteNo = request.SiteNo,
            Block = request.Block,
            Size = request.Area,
            Value = request.PlotValue,
            NormalPrice = request.NormalPrice,
            PromotionPrice = request.PromotionPrice,
            LandTitle = request.LandTitle,
            Description = request.Description,
            Status = request.Status
        };

        var createdPlot = await _plotService.CreateAsync(plot);

        var response = new PlotResponse
        {
            PlotNo = createdPlot.PlotNo,
            SiteNo = createdPlot.SiteNo ?? "",
            SiteName = createdPlot.Site?.SiteName,
            Block = createdPlot.Block,
            Area = createdPlot.Size,
            Value = createdPlot.Value,
            NormalPrice = createdPlot.NormalPrice,
            PromotionPrice = createdPlot.PromotionPrice,
            LandTitle = createdPlot.LandTitle,
            Description = createdPlot.Description,
            Status = createdPlot.Status ?? "",
            DateCreated = createdPlot.DateCreated
        };

        _logger.LogInformation("Plot created: {PlotNo}", createdPlot.PlotNo);
        return CreatedAtAction(nameof(GetByPlotNo), new { plotNo = createdPlot.PlotNo }, ApiResponse.Ok(response, "Plot created successfully"));
    }

    /// <summary>
    /// Update an existing plot
    /// </summary>
    [HttpPut("{plotNo}")]
    [ProducesResponseType(typeof(ApiResponse<PlotResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<PlotResponse>>> Update(string plotNo, [FromBody] PlotUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<PlotResponse>.Fail("Invalid plot data", GetValidationErrors()));

        var plot = new Plot
        {
            Block = request.Block,
            Size = request.Area,
            Value = request.PlotValue,
            NormalPrice = request.NormalPrice ?? 0,
            PromotionPrice = request.PromotionPrice,
            LandTitle = request.LandTitle,
            Description = request.Description,
            Status = request.Status
        };

        var updatedPlot = await _plotService.UpdateAsync(plotNo, plot);

        if (updatedPlot == null)
            return NotFound(ApiResponse<PlotResponse>.Fail($"Plot with number {plotNo} not found"));

        var response = new PlotResponse
        {
            PlotNo = updatedPlot.PlotNo,
            SiteNo = updatedPlot.SiteNo ?? "",
            SiteName = updatedPlot.Site?.SiteName,
            Block = updatedPlot.Block,
            Area = updatedPlot.Size,
            Value = updatedPlot.Value,
            NormalPrice = updatedPlot.NormalPrice,
            PromotionPrice = updatedPlot.PromotionPrice,
            LandTitle = updatedPlot.LandTitle,
            Description = updatedPlot.Description,
            Status = updatedPlot.Status ?? "",
            DateCreated = updatedPlot.DateCreated
        };

        _logger.LogInformation("Plot updated: {PlotNo}", plotNo);
        return Ok(ApiResponse.Ok(response, "Plot updated successfully"));
    }

    /// <summary>
    /// Delete a plot
    /// </summary>
    [HttpDelete("{plotNo}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> Delete(string plotNo)
    {
        try
        {
            var result = await _plotService.DeleteAsync(plotNo);

            if (!result)
                return NotFound(ApiResponse.Fail($"Plot with number {plotNo} not found"));

            _logger.LogInformation("Plot deleted: {PlotNo}", plotNo);
            return Ok(ApiResponse.Ok("Plot deleted successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse.Fail(ex.Message));
        }
    }

    /// <summary>
    /// Search plots
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponse<List<PlotResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<PlotResponse>>>> Search(
        [FromQuery] string searchTerm = "", 
        [FromQuery] string? siteCode = null,
        [FromQuery] string? status = null,
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10)
    {
        var plots = await _plotService.SearchAsync(searchTerm, siteCode, status, page, pageSize);

        var response = plots.Select(p => new PlotResponse
        {
            PlotNo = p.PlotNo,
            SiteNo = p.SiteNo ?? "",
            SiteName = p.Site?.SiteName,
            Block = p.Block,
            Area = p.Size,
            Value = p.Value,
            NormalPrice = p.NormalPrice,
            PromotionPrice = p.PromotionPrice,
            LandTitle = p.LandTitle,
            Description = p.Description,
            Status = p.Status ?? "",
            DateCreated = p.DateCreated
        }).ToList();

        return Ok(ApiResponse.Ok(response));
    }

    /// <summary>
    /// Get available plots
    /// </summary>
    [HttpGet("available")]
    [ProducesResponseType(typeof(ApiResponse<List<PlotResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<PlotResponse>>>> GetAvailablePlots()
    {
        var plots = await _plotService.GetAvailablePlotsAsync();

        var response = plots.Select(p => new PlotResponse
        {
            PlotNo = p.PlotNo,
            SiteNo = p.SiteNo ?? "",
            SiteName = p.Site?.SiteName,
            Block = p.Block,
            Area = p.Size,
            Value = p.Value,
            NormalPrice = p.NormalPrice,
            PromotionPrice = p.PromotionPrice,
            LandTitle = p.LandTitle,
            Description = p.Description,
            Status = p.Status ?? "",
            DateCreated = p.DateCreated
        }).ToList();

        return Ok(ApiResponse.Ok(response));
    }

    /// <summary>
    /// Get plots by site
    /// </summary>
    [HttpGet("site/{siteCode}")]
    [ProducesResponseType(typeof(ApiResponse<List<PlotResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<PlotResponse>>>> GetBySite(string siteCode)
    {
        var plots = await _plotService.GetBySiteAsync(siteCode);

        var response = plots.Select(p => new PlotResponse
        {
            PlotNo = p.PlotNo,
            SiteNo = p.SiteNo ?? "",
            SiteName = p.Site?.SiteName,
            Block = p.Block,
            Area = p.Size,
            Value = p.Value,
            NormalPrice = p.NormalPrice,
            PromotionPrice = p.PromotionPrice,
            LandTitle = p.LandTitle,
            Description = p.Description,
            Status = p.Status ?? "",
            DateCreated = p.DateCreated
        }).ToList();

        return Ok(ApiResponse.Ok(response));
    }

    private List<string> GetValidationErrors()
    {
        return ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();
    }
}
