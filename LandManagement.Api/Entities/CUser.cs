using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandManagement.Api.Entities;

/// <summary>
/// Customer User (CUsers table) - for customer portal access
/// </summary>
[Table("CUsers")]
public class CUser
{
    [Key]
    public int Id { get; set; }
    
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string Password { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string? Email { get; set; }
    
    [StringLength(50)]
    public string? ClientNo { get; set; }
    
    public bool? Status { get; set; }
    
    public DateTime? DateCreated { get; set; }
}
