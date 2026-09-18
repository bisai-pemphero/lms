using System.ComponentModel.DataAnnotations;

namespace LandManagement.Api.DTOs.Plot;

/// <summary>
/// DTO for creating a new plot
/// </summary>
public class PlotCreateRequest
{
    [Required(ErrorMessage = "Plot number is required")]
    [StringLength(50, ErrorMessage = "Plot number cannot exceed 50 characters")]
    public string PlotNo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Site code is required")]
    [StringLength(20, ErrorMessage = "Site code cannot exceed 20 characters")]
    public string SiteNo { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Block cannot exceed 50 characters")]
    public string? Block { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Area must be greater than 0")]
    public decimal? Area { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
    public decimal? Value { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Normal price must be non-negative")]
    public decimal? NormalPrice { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Promotion price must be non-negative")]
    public decimal? PromotionPrice { get; set; }

    [StringLength(50, ErrorMessage = "Land title cannot exceed 50 characters")]
    public string? LandTitle { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
    public string Status { get; set; } = "Available";
}
