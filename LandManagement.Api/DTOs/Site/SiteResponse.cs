namespace LandManagement.Api.DTOs.Site;

/// <summary>
/// DTO for site response with all relevant information
/// </summary>
public class SiteResponse
{
    public string SiteCode { get; set; } = string.Empty;
    public string SiteName { get; set; } = string.Empty;
    public string? District { get; set; }
    public string? PhysicalLocation { get; set; }
    public decimal? Size { get; set; }
    public string? PreviousOwner { get; set; }
    public decimal? InitialValue { get; set; }
    public decimal? AmountPaid { get; set; }
    public decimal? Balance { get; set; }
    public string? DevelopmentsDone { get; set; }
    public decimal? DevelopmentCost { get; set; }
    public string? TA { get; set; }
    public string? Village { get; set; }
    public string? Region { get; set; }
    public string? Description { get; set; }
    public decimal? PricePerSqMeter { get; set; }
    public string? BankName { get; set; }
    public string? AccountNumber { get; set; }
    public decimal? RoadSize { get; set; }
    public decimal? RemainingAcreage { get; set; }
    public string? SiteMap { get; set; }
    public string? Status { get; set; }
    public DateTime? DateCreated { get; set; }

    // Computed properties (optional, can be populated separately)
    public int? TotalPlots { get; set; }
    public int? AvailablePlots { get; set; }
    public int? AllocatedPlots { get; set; }
    public int? SoldPlots { get; set; }
}
