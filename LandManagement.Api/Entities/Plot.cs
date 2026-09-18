using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

[Table("T_Plots")]
public class Plot
{
    [Key]
    [Column("PlotNo")]
    [StringLength(50)]
    public string PlotNo { get; set; } = string.Empty;

    [Column("SiteNo")]
    [StringLength(50)]
    public string? SiteNo { get; set; }

    [Column("Size")]
    public decimal? Size { get; set; }

    [Column("NormalPrice")]
    public decimal NormalPrice { get; set; }

    [Column("PromotionPrice")]
    public decimal? PromotionPrice { get; set; }

    [Column("PlotValue")]
    public decimal? PlotValue { get; set; }

    // Navigation properties
    public virtual Site? Site { get; set; }
    public virtual ICollection<PlotAllocation> PlotAllocations { get; set; } = new List<PlotAllocation>();
    public virtual ICollection<PlotPayment> PlotPayments { get; set; } = new List<PlotPayment>();
}
