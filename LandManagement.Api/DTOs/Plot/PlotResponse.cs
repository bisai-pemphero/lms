namespace LandManagement.Api.DTOs.Plot;

/// <summary>
/// DTO for plot response with all relevant information
/// </summary>
public class PlotResponse
{
    public string PlotNo { get; set; } = string.Empty;
    public string SiteNo { get; set; } = string.Empty;
    public string? SiteName { get; set; } // Populated from related Site
    public string? Block { get; set; }
    public decimal? Area { get; set; }
    public decimal? PlotValue { get; set; }
    public decimal NormalPrice { get; set; }
    public decimal? PromotionPrice { get; set; }
    public string? LandTitle { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? DateCreated { get; set; }

    // Computed properties (optional, can be populated separately)
    public string? ClientNo { get; set; }
    public string? ClientName { get; set; }
    public decimal? AmountPaid { get; set; }
    public decimal? Balance { get; set; }
    public bool IsAvailable => Status.Equals("Available", StringComparison.OrdinalIgnoreCase);
    public bool IsAllocated => Status.Equals("Allocated", StringComparison.OrdinalIgnoreCase);
    public bool IsSold => Status.Equals("Sold", StringComparison.OrdinalIgnoreCase);
}
