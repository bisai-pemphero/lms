using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

[Table("Penalties")]
public class Penalty
{
    [Key]
    public int Id { get; set; }

    [Column("FromRange")]
    public decimal? FromRange { get; set; }

    [Column("ToRange")]
    public decimal? ToRange { get; set; }

    [Column("Charge")]
    public decimal? Charge { get; set; }
}
