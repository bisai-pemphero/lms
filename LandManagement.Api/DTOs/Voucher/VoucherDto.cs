namespace LandManagement.Api.DTOs.Voucher;

/// <summary>
/// Request model for creating a payment voucher
/// </summary>
public class CreateVoucherRequest
{
    public string PayeeName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public bool IsCreditor { get; set; } = false;
}

/// <summary>
/// Response model for a payment voucher
/// </summary>
public class VoucherDto
{
    public int PaymentVoucherId { get; set; }
    public string PayeeName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
}

/// <summary>
/// Request model for approving a voucher
/// </summary>
public class ApproveVoucherRequest
{
    public int VoucherId { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
}
