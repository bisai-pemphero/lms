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

    [Column("Block")]
    [StringLength(50)]
    public string? Block { get; set; }

    [Column("Area")]
    [StringLength(100)]
    public string? Area { get; set; }

    [Column("Size")]
    public decimal? Size { get; set; }

    [Column("NormalPrice")]
    public decimal NormalPrice { get; set; }

    [Column("PromotionPrice")]
    public decimal? PromotionPrice { get; set; }

    [Column("PlotValue")]
    public decimal? Value { get; set; }

    [Column("LandTitle")]
    [StringLength(100)]
    public string? LandTitle { get; set; }

    [Column("Description")]
    [StringLength(500)]
    public string? Description { get; set; }

    [Column("Status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column("OfferDate")]
    public DateTime? OfferDate { get; set; }

    [Column("DateCreated")]
    public DateTime? DateCreated { get; set; }

    // Navigation properties
    public virtual Site? Site { get; set; }
    public virtual ICollection<PlotAllocation> PlotAllocations { get; set; } = new List<PlotAllocation>();
    public virtual ICollection<PlotPayment> PlotPayments { get; set; } = new List<PlotPayment>();
}
