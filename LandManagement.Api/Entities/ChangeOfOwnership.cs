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

    [Column("TransferAmount")]
    public decimal TransferAmount { get; set; }

    [Column("TransferDate")]
    public DateTime TransferDate { get; set; }
}
