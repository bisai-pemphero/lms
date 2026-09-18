using System.ComponentModel.DataAnnotations;

namespace LandManagement.Api.DTOs.Withdrawal;

public class WithdrawalDto
{
    public int Id { get; set; }
    public string PlotNo { get; set; } = string.Empty;
    public string? ClientNo { get; set; }
    public string? ClientName { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal RefundAmount { get; set; }
    public string? Reason { get; set; }
    public DateTime WithdrawalDate { get; set; }
    public string? Status { get; set; }
    public string? PostedBy { get; set; }
}

public class WithdrawalRequest
{
    [Required]
    public string PlotNo { get; set; } = string.Empty;
    
    public string? Reason { get; set; }
    public decimal? RefundAmount { get; set; }
    public DateTime? WithdrawalDate { get; set; }
}

public class WithdrawalResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public WithdrawalDto? Data { get; set; }
}
