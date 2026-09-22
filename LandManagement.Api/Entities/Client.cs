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

    [Column("ClientName")]
    [StringLength(200)]
    public string ClientName { get; set; } = string.Empty;

    [Column("IDNo")]
    [StringLength(50)]
    public string? IdNo { get; set; }

    [Column("PostalAddress")]
    [StringLength(500)]
    public string? PostalAddress { get; set; }

    [Column("PhysicalAddress")]
    [StringLength(500)]
    public string? PhysicalAddress { get; set; }

    [Column("Town")]
    [StringLength(100)]
    public string? Town { get; set; }

    [Column("CellPhone")]
    [StringLength(20)]
    public string? CellPhone { get; set; }

    [Column("Email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("DateCreated")]
    public DateTime? DateCreated { get; set; }

    [Column("CreatedBy")]
    [StringLength(100)]
    public string? CreatedBy { get; set; }

    [Column("ClientType")]
    [StringLength(50)]
    public string? ClientType { get; set; }

    [Column("CompanyRegNo")]
    [StringLength(50)]
    public string? CompanyRegNo { get; set; }

    [Column("PhoneNumber")]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [Column("City")]
    [StringLength(100)]
    public string? City { get; set; }

    [Column("Country")]
    [StringLength(100)]
    public string? Country { get; set; }

    [Column("PostalCode")]
    [StringLength(20)]
    public string? PostalCode { get; set; }

    [Column("CompanyName")]
    [StringLength(200)]
    public string? CompanyName { get; set; }

    [Column("Title")]
    [StringLength(50)]
    public string? Title { get; set; }

    [Column("Comments")]
    [StringLength(500)]
    public string? Comments { get; set; }

    [Column("Status")]
    [StringLength(50)]
    public string? Status { get; set; }

    // Navigation properties
    public virtual ICollection<PlotAllocation> PlotAllocations { get; set; } = new List<PlotAllocation>();
    public virtual ICollection<SaleAgreement> SaleAgreements { get; set; } = new List<SaleAgreement>();
    public virtual ICollection<NextOfKin> NextOfKins { get; set; } = new List<NextOfKin>();
}
