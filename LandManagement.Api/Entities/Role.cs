using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

/// <summary>
/// Role entity representing user roles in the system.
/// Maps to T_Roles table in the legacy database.
/// </summary>
public class Role
{
    [Key]
    [Column("RoleId")]
    public int RoleId { get; set; }

    [Column("RoleName")]
    [StringLength(100)]
    public string RoleName { get; set; } = string.Empty;

    [Column("Description")]
    [StringLength(500)]
    public string? Description { get; set; }

    // Navigation properties
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
