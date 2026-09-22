using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

[Table("T_SaleAgreementPlots")]
public class SaleAgreementPlot
{
    [Key]
    public int Id { get; set; }

    [Column("SaleAgreementId")]
    public int SaleAgreementId { get; set; }

    [Column("PlotNo")]
    [StringLength(50)]
    public string PlotNo { get; set; } = string.Empty;

    [Column("Price")]
    public decimal Price { get; set; }

    [Column("AgreedPrice")]
    public decimal? AgreedPrice { get; set; }

    // Navigation properties
    public virtual SaleAgreement? SaleAgreement { get; set; }
    public virtual Plot? Plot { get; set; }
}
