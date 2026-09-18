using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

[Table("T_ChangeofOwnership")]
public class ChangeOfOwnership
{
    [Key]
    public int Id { get; set; }

    [Column("PreviousOwnerNo")]
    [StringLength(50)]
    public string? PreviousOwnerNo { get; set; }

    [Column("CurrentOwnerNo")]
    [StringLength(50)]
    public string CurrentOwnerNo { get; set; } = string.Empty;

    [Column("PlotNo")]
    [StringLength(50)]
    public string PlotNo { get; set; } = string.Empty;

    [Column("SaleAgreementId")]
    public int? SaleAgreementId { get; set; }

    [Column("TransferAmount")]
    public decimal TransferAmount { get; set; }

    [Column("Balance")]
    public decimal Balance { get; set; }

    [Column("TransferDate")]
    public DateTime TransferDate { get; set; }

    [Column("Status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column("PostedBy")]
    [StringLength(100)]
    public string? PostedBy { get; set; }

    // Navigation properties
    public virtual Plot? Plot { get; set; }
    public virtual Client? PreviousOwner { get; set; }
    public virtual Client? CurrentOwner { get; set; }
}
