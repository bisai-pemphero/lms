using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

/// <summary>
/// Plot entity representing individual plots within sites.
/// Maps to T_Plots table in the legacy database.
/// </summary>
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

    [Column("PlotSize")]
    public decimal? Area { get; set; }

    [Column("PlotValue")]
    public decimal? Value { get; set; }

    [Column("NormalPrice")]
    public decimal? NormalPrice { get; set; }

    [Column("PromotionPrice")]
    public decimal? PromotionPrice { get; set; }

    [Column("LandTitle")]
    [StringLength(50)]
    public string? LandTitle { get; set; }

    [Column("Description")]
    [StringLength(500)]
    public string? Description { get; set; }

    [Column("PlotStatus")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column("OfferDate")]
    [StringLength(50)]
    public string? OfferDate { get; set; }

    [Column("DateCreated")]
    public DateTime? DateCreated { get; set; }

    // Navigation properties
    public virtual Site? Site { get; set; }
    public virtual ICollection<PlotAllocation> PlotAllocations { get; set; } = new List<PlotAllocation>();
    public virtual ICollection<PlotPayment> PlotPayments { get; set; } = new List<PlotPayment>();
}
