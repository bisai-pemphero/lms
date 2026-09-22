using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

[Table("PaymentVoucher")]
public class PaymentVoucher
{
    [Key]
    public int PaymentVoucherId { get; set; }

    [Column("PayeeName")]
    [StringLength(200)]
    public string PayeeName { get; set; } = string.Empty;

    [Column("Description")]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Column("Amount")]
    public decimal? Amount { get; set; }

    [Column("Status")]
    [StringLength(50)]
    public string Status { get; set; } = "Pending";

    [Column("CreatedDate")]
    public DateTime? CreatedDate { get; set; }

    [Column("CreatedBy")]
    [StringLength(100)]
    public string? CreatedBy { get; set; }

    [Column("ApprovedBy")]
    [StringLength(100)]
    public string? ApprovedBy { get; set; }

    [Column("ApprovedDate")]
    public DateTime? ApprovedDate { get; set; }
}
