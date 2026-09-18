using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

/// <summary>
/// PlotPayment entity representing payments made for plot allocations.
/// Maps to T_PlotPayments table in the legacy database.
/// </summary>
public class PlotPayment
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Column("PlotNo")]
    [StringLength(50)]
    public string PlotNo { get; set; } = string.Empty;

    [Column("ClientNo")]
    [StringLength(50)]
    public string? ClientNo { get; set; }

    [Column("AmountPaid")]
    public decimal AmountPaid { get; set; }

    [Column("Balance")]
    public decimal Balance { get; set; }

    [Column("DatePaid")]
    public DateTime DatePaid { get; set; }

    [Column("PaymentMode")]
    [StringLength(50)]
    public string? PaymentMode { get; set; }

    [Column("PaymentRef")]
    [StringLength(100)]
    public string? PaymentRef { get; set; }

    [Column("ReceiptNo")]
    [StringLength(50)]
    public string? ReceiptNo { get; set; }

    [Column("PostedBy")]
    [StringLength(100)]
    public string? PostedBy { get; set; }

    // Navigation properties
    public virtual Plot? Plot { get; set; }
    public virtual Client? Client { get; set; }
    public virtual PlotAllocation? PlotAllocation { get; set; }
}
