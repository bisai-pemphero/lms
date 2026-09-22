using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

[Table("T_PlotPayments")]
public class PlotPayment
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("PlotNo")]
    [StringLength(100)]
    public string PlotNo { get; set; } = string.Empty;

    [Column("SiteNo")]
    [StringLength(100)]
    public string? SiteNo { get; set; }

    [Column("ClientNo")]
    [StringLength(50)]
    public string? ClientNo { get; set; }

    [Column("CurrentBalance")]
    public decimal? CurrentBalance { get; set; }

    [Column("AmountPaid")]
    public decimal AmountPaid { get; set; }

    [Column("NewBalance")]
    public decimal? NewBalance { get; set; }

    [Column("PaidBy")]
    [StringLength(60)]
    public string? PaidBy { get; set; }

    [Column("PaymentMode")]
    [StringLength(60)]
    public string? PaymentMode { get; set; }

    [Column("PaymentReference")]
    [StringLength(40)]
    public string? PaymentReference { get; set; }

    [Column("ReceiptNo")]
    [StringLength(50)]
    public string? ReceiptNo { get; set; }

    [Column("DatePaid")]
    public DateTime? DatePaid { get; set; }

    [Column("PostedBy")]
    [StringLength(60)]
    public string? PostedBy { get; set; }

    // Navigation properties
    public virtual Plot? Plot { get; set; }
    public virtual Client? Client { get; set; }
}
