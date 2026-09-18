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

    [Column("RefundAmount")]
    public decimal RefundAmount { get; set; }

    [Column("Reason")]
    [StringLength(500)]
    public string? Reason { get; set; }

    [Column("WithdrawalDate")]
    public DateTime WithdrawalDate { get; set; }
}
