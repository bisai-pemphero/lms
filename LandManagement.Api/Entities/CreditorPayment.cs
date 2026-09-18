using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

/// <summary>
/// CreditorPayment entity representing payments made to creditors.
/// Maps to T_CreditorPayments table in the legacy database.
/// </summary>
public class CreditorPayment
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Column("CreditorId")]
    public int CreditorId { get; set; }

    [Column("AmountPaid")]
    public decimal AmountPaid { get; set; }

    [Column("DatePaid")]
    public DateTime DatePaid { get; set; }

    [Column("PaymentMode")]
    [StringLength(50)]
    public string? PaymentMode { get; set; }

    [Column("PaymentRef")]
    [StringLength(100)]
    public string? PaymentRef { get; set; }

    [Column("PostedBy")]
    [StringLength(100)]
    public string? PostedBy { get; set; }

    // Navigation properties
    public virtual Creditor Creditor { get; set; } = null!;
}
