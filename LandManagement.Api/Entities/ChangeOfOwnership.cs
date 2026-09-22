using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

[Table("T_ChangeofOwnership")]
public class ChangeOfOwnership
{
    [Key]
    public int Id { get; set; }

    [Column("Previous_Owner")]
    [StringLength(100)]
    public string PreviousOwner { get; set; } = string.Empty;

    [Column("Current_Owner")]
    [StringLength(100)]
    public string CurrentOwner { get; set; } = string.Empty;

    [Column("SaleAgreementId")]
    [StringLength(100)]
    public string SaleAgreementId { get; set; } = string.Empty;

    [Column("ProofofPayment")]
    [StringLength(100)]
    public string ProofofPayment { get; set; } = string.Empty;

    [Column("Date")]
    public DateTime? Date { get; set; }

    // Navigation properties
    public virtual Client? PreviousOwnerNav { get; set; }
    public virtual Client? CurrentOwnerNav { get; set; }
}
