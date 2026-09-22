using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

/// <summary>
/// User entity representing system users.
/// Maps to T_Users table in the legacy database.
/// </summary>
public class User
{
    [Key]
    [Column("UserId")]
    public int UserId { get; set; }

    [Column("Username")]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;

    [Column("PasswordHash")]
    [StringLength(500)]
    public string PasswordHash { get; set; } = string.Empty;

    // Legacy password property for backward compatibility
    [NotMapped]
    public string Password => PasswordHash;

    [Column("FullName")]
    [StringLength(200)]
    public string? FullName { get; set; }

    [Column("Email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("RoleId")]
    public int? RoleId { get; set; }

    [Column("IsActive")]
    public bool IsActive { get; set; } = true;

    [Column("DateCreated")]
    public DateTime? DateCreated { get; set; }

    [Column("LastLogin")]
    public DateTime? LastLogin { get; set; }

    // Navigation properties
    public virtual Role? Role { get; set; }
}
