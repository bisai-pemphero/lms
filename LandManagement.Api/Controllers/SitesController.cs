using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LandManagement.Api.DTOs.Site;
using LandManagement.Api.Entities;
using LandManagement.Api.Services;
using LandManagement.Api.Shared.Models;

namespace LandManagement.Api.Controllers;

/// <summary>
/// Controller for site operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SitesController : ControllerBase
{
    private readonly ISiteService _siteService;
    private readonly ILogger<SitesController> _logger;

    public SitesController(ISiteService siteService, ILogger<SitesController> logger)
    {
        _siteService = siteService;
        _logger = logger;
    }

    /// <summary>
    /// Get all sites
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<SiteResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<SiteResponse>>>> GetAll()
    {
        var sites = await _siteService.GetAllAsync();
        
        var response = sites.Select(s => new SiteResponse
        {
            SiteCode = s.SiteCode,
            SiteName = s.SiteName,
            District = s.District,
            PhysicalLocation = s.PhysicalLocation,
            Size = s.Size,
            PreviousOwner = s.PreviousOwner,
            InitialValue = s.InitialValue,
            AmountPaid = s.AmountPaid,
            Balance = s.Balance,
            DevelopmentsDone = s.DevelopmentsDone,
            DevelopmentCost = s.DevelopmentCost,
            TA = s.TA,
            Village = s.Village,
            Region = s.Region,
            Description = s.Description,
            PricePerSqMeter = s.PricePerSqMeter,
            BankName = s.BankName,
            AccountNumber = s.AccountNumber,
            RoadSize = s.RoadSize,
            RemainingAcreage = s.RemainingAcreage,
            SiteMap = s.SiteMap,
            Status = s.Status,
            DateCreated = s.DateCreated
        }).ToList();

        return Ok(ApiResponse.Ok(response));
    }

    /// <summary>
    /// Get site by site code
    /// </summary>
    [HttpGet("{siteCode}")]
    [ProducesResponseType(typeof(ApiResponse<SiteResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<SiteResponse>>> GetBySiteCode(string siteCode)
    {
        var site = await _siteService.GetBySiteCodeAsync(siteCode);

        if (site == null)
            return NotFound(ApiResponse<SiteResponse>.Fail($"Site with code {siteCode} not found"));

        var response = new SiteResponse
        {
            SiteCode = site.SiteCode,
            SiteName = site.SiteName,
            District = site.District,
            PhysicalLocation = site.PhysicalLocation,
            Size = site.Size,
            PreviousOwner = site.PreviousOwner,
            InitialValue = site.InitialValue,
            AmountPaid = site.AmountPaid,
            Balance = site.Balance,
            DevelopmentsDone = site.DevelopmentsDone,
            DevelopmentCost = site.DevelopmentCost,
            TA = site.TA,
            Village = site.Village,
            Region = site.Region,
            Description = site.Description,
            PricePerSqMeter = site.PricePerSqMeter,
            BankName = site.BankName,
            AccountNumber = site.AccountNumber,
            RoadSize = site.RoadSize,
            RemainingAcreage = site.RemainingAcreage,
            SiteMap = site.SiteMap,
            Status = site.Status,
            DateCreated = site.DateCreated
        };

        return Ok(ApiResponse.Ok(response));
    }

    /// <summary>
    /// Create a new site
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<SiteResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<SiteResponse>>> Create([FromBody] SiteCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<SiteResponse>.Fail("Invalid site data", GetValidationErrors()));

        var site = new Site
        {
            SiteCode = request.SiteCode,
            SiteName = request.SiteName,
            District = request.District,
            PhysicalLocation = request.PhysicalLocation,
            Size = request.Size,
            PreviousOwner = request.PreviousOwner,
            InitialValue = request.InitialValue,
            AmountPaid = request.AmountPaid,
            Balance = request.Balance,
            DevelopmentsDone = request.DevelopmentsDone,
            DevelopmentCost = request.DevelopmentCost,
            TA = request.TA,
            Village = request.Village,
            Region = request.Region,
            Description = request.Description,
            PricePerSqMeter = request.PricePerSqMeter,
            BankName = request.BankName,
            AccountNumber = request.AccountNumber,
            RoadSize = request.RoadSize,
            RemainingAcreage = request.RemainingAcreage,
            SiteMap = request.SiteMap
        };

        var createdSite = await _siteService.CreateAsync(site);

        var response = new SiteResponse
        {
            SiteCode = createdSite.SiteCode,
            SiteName = createdSite.SiteName,
            District = createdSite.District,
            PhysicalLocation = createdSite.PhysicalLocation,
            Size = createdSite.Size,
            PreviousOwner = createdSite.PreviousOwner,
            InitialValue = createdSite.InitialValue,
            AmountPaid = createdSite.AmountPaid,
            Balance = createdSite.Balance,
            DevelopmentsDone = createdSite.DevelopmentsDone,
            DevelopmentCost = createdSite.DevelopmentCost,
            TA = createdSite.TA,
            Village = createdSite.Village,
            Region = createdSite.Region,
            Description = createdSite.Description,
            PricePerSqMeter = createdSite.PricePerSqMeter,
            BankName = createdSite.BankName,
            AccountNumber = createdSite.AccountNumber,
            RoadSize = createdSite.RoadSize,
            RemainingAcreage = createdSite.RemainingAcreage,
            SiteMap = createdSite.SiteMap,
            Status = createdSite.Status,
            DateCreated = createdSite.DateCreated
        };

        _logger.LogInformation("Site created: {SiteCode}", createdSite.SiteCode);
        return CreatedAtAction(nameof(GetBySiteCode), new { siteCode = createdSite.SiteCode }, ApiResponse.Ok(response, "Site created successfully"));
    }

    /// <summary>
    /// Update an existing site
    /// </summary>
    [HttpPut("{siteCode}")]
    [ProducesResponseType(typeof(ApiResponse<SiteResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<SiteResponse>>> Update(string siteCode, [FromBody] SiteUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<SiteResponse>.Fail("Invalid site data", GetValidationErrors()));

        var site = new Site
        {
            SiteName = request.SiteName,
            District = request.District,
            PhysicalLocation = request.PhysicalLocation,
            Size = request.Size,
            PreviousOwner = request.PreviousOwner,
            InitialValue = request.InitialValue,
            AmountPaid = request.AmountPaid,
            Balance = request.Balance,
            DevelopmentsDone = request.DevelopmentsDone,
            DevelopmentCost = request.DevelopmentCost,
            TA = request.TA,
            Village = request.Village,
            Region = request.Region,
            Description = request.Description,
            PricePerSqMeter = request.PricePerSqMeter,
            BankName = request.BankName,
            AccountNumber = request.AccountNumber,
            RoadSize = request.RoadSize,
            RemainingAcreage = request.RemainingAcreage,
            SiteMap = request.SiteMap,
            Status = request.Status
        };

        var updatedSite = await _siteService.UpdateAsync(siteCode, site);

        if (updatedSite == null)
            return NotFound(ApiResponse<SiteResponse>.Fail($"Site with code {siteCode} not found"));

        var response = new SiteResponse
        {
            SiteCode = updatedSite.SiteCode,
            SiteName = updatedSite.SiteName,
            District = updatedSite.District,
            PhysicalLocation = updatedSite.PhysicalLocation,
            Size = updatedSite.Size,
            PreviousOwner = updatedSite.PreviousOwner,
            InitialValue = updatedSite.InitialValue,
            AmountPaid = updatedSite.AmountPaid,
            Balance = updatedSite.Balance,
            DevelopmentsDone = updatedSite.DevelopmentsDone,
            DevelopmentCost = updatedSite.DevelopmentCost,
            TA = updatedSite.TA,
            Village = updatedSite.Village,
            Region = updatedSite.Region,
            Description = updatedSite.Description,
            PricePerSqMeter = updatedSite.PricePerSqMeter,
            BankName = updatedSite.BankName,
            AccountNumber = updatedSite.AccountNumber,
            RoadSize = updatedSite.RoadSize,
            RemainingAcreage = updatedSite.RemainingAcreage,
            SiteMap = updatedSite.SiteMap,
            Status = updatedSite.Status,
            DateCreated = updatedSite.DateCreated
        };

        _logger.LogInformation("Site updated: {SiteCode}", siteCode);
        return Ok(ApiResponse.Ok(response, "Site updated successfully"));
    }

    /// <summary>
    /// Delete a site
    /// </summary>
    [HttpDelete("{siteCode}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> Delete(string siteCode)
    {
        try
        {
            var result = await _siteService.DeleteAsync(siteCode);

            if (!result)
                return NotFound(ApiResponse.Fail($"Site with code {siteCode} not found"));

            _logger.LogInformation("Site deleted: {SiteCode}", siteCode);
            return Ok(ApiResponse.Ok("Site deleted successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse.Fail(ex.Message));
        }
    }

    /// <summary>
    /// Search sites
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponse<List<SiteResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<SiteResponse>>>> Search(
        [FromQuery] string searchTerm = "", 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10)
    {
        var sites = await _siteService.SearchAsync(searchTerm, page, pageSize);

        var response = sites.Select(s => new SiteResponse
        {
            SiteCode = s.SiteCode,
            SiteName = s.SiteName,
            District = s.District,
            PhysicalLocation = s.PhysicalLocation,
            Size = s.Size,
            PreviousOwner = s.PreviousOwner,
            InitialValue = s.InitialValue,
            AmountPaid = s.AmountPaid,
            Balance = s.Balance,
            DevelopmentsDone = s.DevelopmentsDone,
            DevelopmentCost = s.DevelopmentCost,
            TA = s.TA,
            Village = s.Village,
            Region = s.Region,
            Description = s.Description,
            PricePerSqMeter = s.PricePerSqMeter,
            BankName = s.BankName,
            AccountNumber = s.AccountNumber,
            RoadSize = s.RoadSize,
            RemainingAcreage = s.RemainingAcreage,
            SiteMap = s.SiteMap,
            Status = s.Status
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
