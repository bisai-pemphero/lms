using System.ComponentModel.DataAnnotations;

namespace LandManagement.Api.DTOs.PlotAllocation;

public class PlotAllocationDto
{
    public int Id { get; set; }
    public string ClientNo { get; set; } = string.Empty;
    public string? ClientName { get; set; }
    public string PlotNo { get; set; } = string.Empty;
    public string? SiteCode { get; set; }
    public decimal AgreedPrice { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal Balance { get; set; }
    public decimal? MonthlyInstallment { get; set; }
    public DateTime AllocationDate { get; set; }
    public string? PostedBy { get; set; }
    public string PlotStatus { get; set; } = string.Empty;
    public string? Status { get; set; }
}

public class PlotAllocationRequest
{
    [Required]
    public string ClientNo { get; set; } = string.Empty;
    
    [Required]
    public string PlotNo { get; set; } = string.Empty;
    
    public decimal AgreedPrice { get; set; }
    public decimal? MonthlyInstallment { get; set; }
    public DateTime? AllocationDate { get; set; }
}

public class PlotAllocationResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public PlotAllocationDto? Data { get; set; }
}
