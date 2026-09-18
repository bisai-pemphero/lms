using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

/// <summary>
/// Site entity representing land sites in the management system.
/// Maps to T_Sites table in the legacy database.
/// </summary>
public class Site
{
    [Key]
    [Column("SiteCode")]
    [StringLength(50)]
    public string SiteCode { get; set; } = string.Empty;

    [Column("SiteName")]
    [StringLength(100)]
    public string SiteName { get; set; } = string.Empty;

    [Column("District")]
    [StringLength(100)]
    public string? District { get; set; }

    [Column("PhysicalLocation")]
    [StringLength(500)]
    public string? PhysicalLocation { get; set; }

    [Column("Size")]
    public decimal? Size { get; set; }

    [Column("PreviousOwner")]
    [StringLength(200)]
    public string? PreviousOwner { get; set; }

    [Column("InitialValue")]
    public decimal? InitialValue { get; set; }

    [Column("AmountPaid")]
    public decimal? AmountPaid { get; set; }

    [Column("Balance")]
    public decimal? Balance { get; set; }

    [Column("DevelopmentsDone")]
    [StringLength(500)]
    public string? DevelopmentsDone { get; set; }

    [Column("DevelopmentCost")]
    public decimal? DevelopmentCost { get; set; }

    [Column("TA")]
    [StringLength(100)]
    public string? TA { get; set; }

    [Column("Village")]
    [StringLength(100)]
    public string? Village { get; set; }

    [Column("Region")]
    [StringLength(100)]
    public string? Region { get; set; }

    [Column("Description")]
    [StringLength(1000)]
    public string? Description { get; set; }

    [Column("PricePerSqMeter")]
    public decimal? PricePerSqMeter { get; set; }

    [Column("BankName")]
    [StringLength(200)]
    public string? BankName { get; set; }

    [Column("AccountNumber")]
    [StringLength(50)]
    public string? AccountNumber { get; set; }

    [Column("RoadSize")]
    public decimal? RoadSize { get; set; }

    [Column("RemainingAcreage")]
    public decimal? RemainingAcreage { get; set; }

    [Column("SiteMap")]
    [StringLength(500)]
    public string? SiteMap { get; set; }

    [Column("Status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column("DateCreated")]
    public DateTime? DateCreated { get; set; }

    // Navigation properties
    public virtual ICollection<Plot> Plots { get; set; } = new List<Plot>();
}
