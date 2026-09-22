using System.ComponentModel.DataAnnotations;

namespace LandManagement.Api.DTOs.SaleAgreement;

public class SaleAgreementDto
{
    public int Id { get; set; }
    public string AgreementNo { get; set; } = string.Empty;
    public string ClientNo { get; set; } = string.Empty;
    public string? ClientName { get; set; }
    public DateTime? AgreementDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal Balance { get; set; }
    public string? Status { get; set; }
    public string? PostedBy { get; set; }
    public List<string> PlotNos { get; set; } = new();
}

public class SaleAgreementRequest
{
    [Required]
    public string ClientNo { get; set; } = string.Empty;
    
    [Required]
    public List<string> PlotNos { get; set; } = new();
    
    public decimal? TotalAmount { get; set; }
    public DateTime? AgreementDate { get; set; }
}

public class SaleAgreementResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public SaleAgreementDto? Data { get; set; }
}
