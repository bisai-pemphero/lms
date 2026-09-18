using System.ComponentModel.DataAnnotations;

namespace LandManagement.Api.DTOs.Plot;

/// <summary>
/// DTO for updating an existing plot
/// </summary>
public class PlotUpdateRequest
{
    [StringLength(50, ErrorMessage = "Block cannot exceed 50 characters")]
    public string? Block { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Area must be greater than 0")]
    public decimal? Area { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Plot value must be non-negative")]
    public decimal? PlotValue { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Normal price must be non-negative")]
    public decimal? NormalPrice { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Promotion price must be non-negative")]
    public decimal? PromotionPrice { get; set; }

    [StringLength(50, ErrorMessage = "Land title cannot exceed 50 characters")]
    public string? LandTitle { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
    public string? Status { get; set; }
}
