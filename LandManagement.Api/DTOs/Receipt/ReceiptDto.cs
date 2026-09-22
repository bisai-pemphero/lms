namespace LandManagement.Api.DTOs.Receipt;

/// <summary>
/// Response model for a receipt
/// </summary>
public class ReceiptDto
{
    public string ReceiptNo { get; set; } = string.Empty;
    public string PlotNo { get; set; } = string.Empty;
    public decimal AmountPaid { get; set; }
    public decimal NewBalance { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string PaymentMode { get; set; } = string.Empty;
    public DateTime DatePaid { get; set; }
    public string PostedBy { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public string Site { get; set; } = string.Empty;
    public decimal PlotValue { get; set; }
    public decimal TotalPaid { get; set; }
    public string CustomerAddress { get; set; } = string.Empty;
}

/// <summary>
/// Request model for querying receipts
/// </summary>
public class ReceiptQueryRequest
{
    public string? ReceiptNo { get; set; }
    public string? PlotNo { get; set; }
    public string? CustomerName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
