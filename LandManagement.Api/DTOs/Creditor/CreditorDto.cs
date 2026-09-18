using System.ComponentModel.DataAnnotations;

namespace LandManagement.Api.DTOs.Creditor;

public class CreditorDto
{
    public int Id { get; set; }
    public int CreditorId { get; set; }
    public string CreditorName { get; set; } = string.Empty;
    public string? SiteCode { get; set; }
    public decimal AmountAgreed { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal Balance { get; set; }
    public string? Status { get; set; }
    public string? PostedBy { get; set; }
}

public class CreditorRequest
{
    [Required]
    public string CreditorName { get; set; } = string.Empty;
    
    public string? SiteCode { get; set; }
    public decimal AmountAgreed { get; set; }
}

public class CreditorResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public CreditorDto? Data { get; set; }
}
