using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

[Table("T_Creditors")]
public class Creditor
{
    [Key]
    public int Id { get; set; }

    [Column("CreditorName")]
    [StringLength(100)]
    public string CreditorName { get; set; } = string.Empty;

    [Column("SiteCode")]
    [StringLength(50)]
    public string? SiteCode { get; set; }

    [Column("AmountAgreed")]
    public decimal AmountAgreed { get; set; }

    [Column("AmountPaid")]
    public decimal AmountPaid { get; set; }

    [Column("Balance")]
    public decimal Balance { get; set; }
}
