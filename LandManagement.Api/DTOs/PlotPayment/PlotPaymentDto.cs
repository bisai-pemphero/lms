using System.ComponentModel.DataAnnotations;

namespace LandManagement.Api.DTOs.PlotPayment;

public class PlotPaymentDto
{
    public int Id { get; set; }
    public string PlotNo { get; set; } = string.Empty;
    public string? ClientNo { get; set; }
    public string? ClientName { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal Balance { get; set; }
    public DateTime DatePaid { get; set; }
    public string? PaymentMode { get; set; }
    public string? ReceiptNumber { get; set; }
    public string? PostedBy { get; set; }
}

public class PlotPaymentRequest
{
    [Required]
    public string PlotNo { get; set; } = string.Empty;
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal AmountPaid { get; set; }
    
    public string? PaymentMode { get; set; }
    public DateTime? DatePaid { get; set; }
}

public class PlotPaymentResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public PlotPaymentDto? Data { get; set; }
}
