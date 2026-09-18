using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

/// <summary>
/// Creditor entity representing creditors in the land management system.
/// Maps to T_Creditors table in the legacy database.
/// </summary>
public class Creditor
{
    [Key]
    [Column("CreditorId")]
    public int CreditorId { get; set; }

    [Column("CreditorName")]
    [StringLength(200)]
    public string CreditorName { get; set; } = string.Empty;

    [Column("LandOwner")]
    [StringLength(200)]
    public string? LandOwner { get; set; }

    [Column("SiteCode")]
    [StringLength(50)]
    public string? SiteCode { get; set; }

    [Column("AmountAgreed")]
    public decimal AmountAgreed { get; set; }

    [Column("AmountPaid")]
    public decimal AmountPaid { get; set; }

    [Column("Balance")]
    public decimal Balance { get; set; }

    [Column("RequestedBy")]
    [StringLength(100)]
    public string? RequestedBy { get; set; }

    [Column("CheckedbyOpm")]
    [StringLength(100)]
    public string? CheckedbyOpm { get; set; }

    [Column("AuthorisedByCEO")]
    [StringLength(100)]
    public string? AuthorisedByCeo { get; set; }

    [Column("FinanceLedgeby")]
    [StringLength(100)]
    public string? FinanceLedgeby { get; set; }

    [Column("Postedby")]
    [StringLength(100)]
    public string? Postedby { get; set; }

    [Column("DateCreated")]
    public DateTime? DateCreated { get; set; }

    // Navigation properties
    public virtual Site? Site { get; set; }
    public virtual ICollection<CreditorPayment> CreditorPayments { get; set; } = new List<CreditorPayment>();
}
