using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

[Table("T_PlotWithdrawal")]
public class PlotWithdrawal
{
    [Key]
    public int Id { get; set; }

    [Column("PlotNo")]
    [StringLength(50)]
    public string PlotNo { get; set; } = string.Empty;

    [Column("ClientNo")]
    [StringLength(50)]
    public string ClientNo { get; set; } = string.Empty;

    [Column("AmountPaid")]
    public decimal AmountPaid { get; set; }

    [Column("Balance")]
    public decimal Balance { get; set; }

    [Column("RefundAmount")]
    public decimal RefundAmount { get; set; }

    [Column("Reason")]
    [StringLength(500)]
    public string? Reason { get; set; }

    [Column("Status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column("CollectedBy")]
    [StringLength(100)]
    public string? CollectedBy { get; set; }

    [Column("WithdrawalDate")]
    public DateTime WithdrawalDate { get; set; }

    [Column("PostedBy")]
    [StringLength(100)]
    public string? PostedBy { get; set; }

    // Navigation properties
    public virtual Plot? Plot { get; set; }
    public virtual Client? Client { get; set; }
}
