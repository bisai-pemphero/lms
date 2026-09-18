using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

[Table("T_SaleAgreements")]
public class SaleAgreement
{
    [Key]
    public int Id { get; set; }

    [Column("AgreementNo")]
    [StringLength(50)]
    public string AgreementNo { get; set; } = string.Empty;

    [Column("ClientNo")]
    [StringLength(50)]
    public string ClientNo { get; set; } = string.Empty;

    [Column("TotalAmount")]
    public decimal TotalAmount { get; set; }

    [Column("AmountPaid")]
    public decimal AmountPaid { get; set; }

    [Column("Balance")]
    public decimal Balance { get; set; }

    [Column("DateCreated")]
    public DateTime? DateCreated { get; set; }

    [Column("AgreementDate")]
    public DateTime? AgreementDate { get; set; }

    [Column("PostedBy")]
    [StringLength(100)]
    public string? PostedBy { get; set; }

    [Column("Status")]
    [StringLength(50)]
    public string? Status { get; set; }

    // Navigation properties
    public virtual Client? Client { get; set; }
    public virtual ICollection<SaleAgreementPlot> SaleAgreementPlots { get; set; } = new List<SaleAgreementPlot>();
}
