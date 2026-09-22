using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

/// <summary>
/// Client entity representing customers in the land management system.
/// Maps to T_Clients table in the legacy database.
/// </summary>
public class Client
{
    [Key]
    [Column("ClientNo")]
    [StringLength(50)]
    public string ClientNo { get; set; } = string.Empty;

    [Column("Fullname")]
    [StringLength(100)]
    public string Fullname { get; set; } = string.Empty;

    [Column("District")]
    [StringLength(50)]
    public string? District { get; set; }

    [Column("Address")]
    [StringLength(500)]
    public string? Address { get; set; }

    [Column("PhoneNo")]
    [StringLength(100)]
    public string? PhoneNo { get; set; }

    [Column("PhoneNo2")]
    [StringLength(100)]
    public string? PhoneNo2 { get; set; }

    [Column("Email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("Occupation")]
    [StringLength(80)]
    public string? Occupation { get; set; }

    [Column("IdentityNo")]
    [StringLength(30)]
    public string? IdentityNo { get; set; }

    [Column("PostedBy")]
    [StringLength(60)]
    public string? PostedBy { get; set; }

    // Navigation properties
    public virtual ICollection<PlotAllocation> PlotAllocations { get; set; } = new List<PlotAllocation>();
    public virtual ICollection<SaleAgreement> SaleAgreements { get; set; } = new List<SaleAgreement>();
    public virtual ICollection<NextOfKin> NextOfKins { get; set; } = new List<NextOfKin>();
}
