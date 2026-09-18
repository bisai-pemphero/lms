using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

/// <summary>
/// NextOfKin entity representing client's next of kin information.
/// Maps to T_Next_of_Kins table in the legacy database.
/// </summary>
public class NextOfKin
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Column("ClientNo")]
    [StringLength(50)]
    public string ClientNo { get; set; } = string.Empty;

    [Column("Name")]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Column("Relationship")]
    [StringLength(100)]
    public string? Relationship { get; set; }

    [Column("Contact")]
    [StringLength(50)]
    public string? Contact { get; set; }

    [Column("Address")]
    [StringLength(500)]
    public string? Address { get; set; }

    // Navigation properties
    public virtual Client Client { get; set; } = null!;
}
