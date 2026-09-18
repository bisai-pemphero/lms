using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

/// <summary>
/// PlotAllocation entity representing the allocation of plots to clients.
/// Maps to T_PlotAllocations table in the legacy database.
/// </summary>
public class PlotAllocation
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Column("ClientNo")]
    [StringLength(50)]
    public string ClientNo { get; set; } = string.Empty;

    [Column("PlotNo")]
    [StringLength(50)]
    public string PlotNo { get; set; } = string.Empty;

    [Column("AgreedPrice")]
    public decimal AgreedPrice { get; set; }

    [Column("AmountPaid")]
    public decimal AmountPaid { get; set; }

    [Column("Balance")]
    public decimal Balance { get; set; }

    [Column("MonthlyInstallment")]
    public decimal? MonthlyInstallment { get; set; }

    [Column("AllocationDate")]
    public DateTime AllocationDate { get; set; }

    [Column("CreatedOn")]
    public DateTime? CreatedOn { get; set; }

    [Column("Status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column("PostedBy")]
    [StringLength(100)]
    public string? PostedBy { get; set; }

    // Navigation properties
    public virtual Client Client { get; set; } = null!;
    public virtual Plot Plot { get; set; } = null!;
    public virtual ICollection<PlotPayment> PlotPayments { get; set; } = new List<PlotPayment>();
}
