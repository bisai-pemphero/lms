using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

/// <summary>
/// SaleAgreementPlot entity representing plots associated with sale agreements.
/// Maps to T_SaleAgreementPlots table in the legacy database.
/// </summary>
public class SaleAgreementPlot
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Column("SaleAgreementId")]
    public int SaleAgreementId { get; set; }

    [Column("PlotNo")]
    [StringLength(50)]
    public string PlotNo { get; set; } = string.Empty;

    [Column("Price")]
    public decimal Price { get; set; }

    // Navigation properties
    public virtual SaleAgreement SaleAgreement { get; set; } = null!;
    public virtual Plot Plot { get; set; } = null!;
}
