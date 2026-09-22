using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

[Table("T_Creditors")]
public class Creditor
{
    [Key]
    public int CreditorId { get; set; }

    [Column("CreditorName")]
    [StringLength(100)]
    public string CreditorName { get; set; } = string.Empty;

    [Column("SiteCode")]
    [StringLength(50)]
    public string? SiteCode { get; set; }

    [Column("LandOwner")]
    [StringLength(100)]
    public string? LandOwner { get; set; }

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

    [Column("Status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column("PostedBy")]
    [StringLength(100)]
    public string? PostedBy { get; set; }

    // Navigation properties
    public virtual Site? Site { get; set; }
    public virtual ICollection<CreditorPayment> CreditorPayments { get; set; } = new List<CreditorPayment>();
}
