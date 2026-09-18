using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

[Table("T_PlotPayments")]
public class PlotPayment
{
    [Key]
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
    [StringLength(100)]
    public string? ReceiptNo { get; set; }

    [Column("ReceiptNumber")]
    [StringLength(100)]
    public string? ReceiptNumber { get; set; }

    [Column("PostedBy")]
    [StringLength(100)]
    public string? PostedBy { get; set; }

    // Navigation properties
    public virtual Plot? Plot { get; set; }
    public virtual Client? Client { get; set; }
}
