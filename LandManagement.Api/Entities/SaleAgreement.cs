using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

/// <summary>
/// SaleAgreement entity representing sale agreements for plots.
/// Maps to T_SaleAgreements table in the legacy database.
/// </summary>
public class SaleAgreement
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Column("AgreementNo")]
    [StringLength(50)]
    public string? AgreementNo { get; set; }

    [Column("ClientNo")]
    [StringLength(50)]
    public string ClientNo { get; set; } = string.Empty;

    [Column("DateCreated")]
    public DateTime? DateCreated { get; set; }

    [Column("Status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column("TotalAmount")]
    public decimal? TotalAmount { get; set; }

    [Column("AmountPaid")]
    public decimal? AmountPaid { get; set; }

    [Column("Balance")]
    public decimal? Balance { get; set; }

    // Navigation properties
    public virtual Client Client { get; set; } = null!;
    public virtual ICollection<SaleAgreementPlot> SaleAgreementPlots { get; set; } = new List<SaleAgreementPlot>();
}
