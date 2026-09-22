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

    [Column("PlotSize")]
    [StringLength(50)]
    public string? PlotSize { get; set; }

    [Column("PriceCategory")]
    [StringLength(60)]
    public string? PriceCategory { get; set; }

    [Column("PlotStatus")]
    [StringLength(60)]
    public string? PlotStatus { get; set; }

    [Column("OfferedTo")]
    [StringLength(40)]
    public string? OfferedTo { get; set; }

    [Column("DateOffered")]
    public DateTime? DateOffered { get; set; }

    [Column("AgreedPrice")]
    public decimal? AgreedPrice { get; set; }

    [Column("AmountPaid")]
    public decimal? AmountPaid { get; set; }

    [Column("Balance")]
    public decimal? Balance { get; set; }

    [Column("FullPaymentDate")]
    public DateTime? FullPaymentDate { get; set; }

    [Column("DateRegistered")]
    public DateTime? DateRegistered { get; set; }

    [Column("RegisteredBy")]
    [StringLength(60)]
    public string? RegisteredBy { get; set; }

    [Column("OfferPeriod")]
    [StringLength(50)]
    public string? OfferPeriod { get; set; }

    [Column("MonthlyInstallment")]
    public decimal? MonthlyInstallment { get; set; }

    [Column("PlotSketch")]
    [StringLength(100)]
    public string? PlotSketch { get; set; }

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
