using System.ComponentModel.DataAnnotations;

namespace LandManagement.Api.DTOs.Client;

/// <summary>
/// DTO for updating an existing client matching legacy T_Clients table schema
/// </summary>
public class ClientUpdateRequest
{
    [Required(ErrorMessage = "Fullname is required")]
    [StringLength(100, ErrorMessage = "Fullname cannot exceed 100 characters")]
    public string Fullname { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "District cannot exceed 50 characters")]
    public string? District { get; set; }

    [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
    public string? Address { get; set; }

    [StringLength(100, ErrorMessage = "PhoneNo cannot exceed 100 characters")]
    public string? PhoneNo { get; set; }

    [StringLength(100, ErrorMessage = "PhoneNo2 cannot exceed 100 characters")]
    public string? PhoneNo2 { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string? Email { get; set; }

    [StringLength(80, ErrorMessage = "Occupation cannot exceed 80 characters")]
    public string? Occupation { get; set; }

    [StringLength(30, ErrorMessage = "IdentityNo cannot exceed 30 characters")]
    public string? IdentityNo { get; set; }

    [StringLength(60, ErrorMessage = "PostedBy cannot exceed 60 characters")]
    public string? PostedBy { get; set; }
}
