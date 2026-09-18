using System.ComponentModel.DataAnnotations;

namespace LandManagement.Api.DTOs.Site;

/// <summary>
/// DTO for creating a new site
/// </summary>
public class SiteCreateRequest
{
    [Required(ErrorMessage = "Site code is required")]
    [StringLength(20, ErrorMessage = "Site code cannot exceed 20 characters")]
    public string SiteCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Site name is required")]
    [StringLength(100, ErrorMessage = "Site name cannot exceed 100 characters")]
    public string SiteName { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "District cannot exceed 100 characters")]
    public string? District { get; set; }

    [StringLength(200, ErrorMessage = "Physical location cannot exceed 200 characters")]
    public string? PhysicalLocation { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Size must be greater than 0")]
    public decimal? Size { get; set; }

    [StringLength(100, ErrorMessage = "Previous owner cannot exceed 100 characters")]
    public string? PreviousOwner { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Initial value must be non-negative")]
    public decimal? InitialValue { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Amount paid must be non-negative")]
    public decimal? AmountPaid { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Balance must be non-negative")]
    public decimal? Balance { get; set; }

    [StringLength(500, ErrorMessage = "Developments done cannot exceed 500 characters")]
    public string? DevelopmentsDone { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Development cost must be non-negative")]
    public decimal? DevelopmentCost { get; set; }

    [StringLength(100, ErrorMessage = "TA cannot exceed 100 characters")]
    public string? TA { get; set; }

    [StringLength(100, ErrorMessage = "Village cannot exceed 100 characters")]
    public string? Village { get; set; }

    [StringLength(100, ErrorMessage = "Region cannot exceed 100 characters")]
    public string? Region { get; set; }

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string? Description { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Price per square meter must be non-negative")]
    public decimal? PricePerSqMeter { get; set; }

    [StringLength(200, ErrorMessage = "Bank name cannot exceed 200 characters")]
    public string? BankName { get; set; }

    [StringLength(50, ErrorMessage = "Account number cannot exceed 50 characters")]
    public string? AccountNumber { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Road size must be non-negative")]
    public decimal? RoadSize { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Remaining acreage must be non-negative")]
    public decimal? RemainingAcreage { get; set; }

    [StringLength(500, ErrorMessage = "Site map path cannot exceed 500 characters")]
    public string? SiteMap { get; set; }
}
