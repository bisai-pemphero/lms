using System.ComponentModel.DataAnnotations;

namespace LandManagement.Api.DTOs.ChangeOfOwnership;

public class ChangeOfOwnershipDto
{
    public int Id { get; set; }
    public string? PreviousOwnerNo { get; set; }
    public string? PreviousOwnerName { get; set; }
    public string CurrentOwnerNo { get; set; } = string.Empty;
    public string? CurrentOwnerName { get; set; }
    public string PlotNo { get; set; } = string.Empty;
    public decimal TransferAmount { get; set; }
    public DateTime TransferDate { get; set; }
    public string? Status { get; set; }
    public string? PostedBy { get; set; }
}

public class ChangeOfOwnershipRequest
{
    [Required]
    public string PreviousOwnerNo { get; set; } = string.Empty;
    
    [Required]
    public string CurrentOwnerNo { get; set; } = string.Empty;
    
    [Required]
    public string PlotNo { get; set; } = string.Empty;
    
    public decimal? TransferAmount { get; set; }
    public DateTime? TransferDate { get; set; }
}

public class ChangeOfOwnershipResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public ChangeOfOwnershipDto? Data { get; set; }
}
