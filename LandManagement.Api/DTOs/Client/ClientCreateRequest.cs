using System.ComponentModel.DataAnnotations;

namespace LandManagement.Api.DTOs.Client;

/// <summary>
/// DTO for creating a new client
/// </summary>
public class ClientCreateRequest
{
    [Required(ErrorMessage = "Client name is required")]
    [StringLength(100, ErrorMessage = "Client name cannot exceed 100 characters")]
    public string ClientName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone number format")]
    [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
    public string? PhoneNumber { get; set; }

    [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
    public string? PhysicalAddress { get; set; }

    [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
    public string? City { get; set; }

    [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
    public string? Country { get; set; }

    [StringLength(20, ErrorMessage = "Postal code cannot exceed 20 characters")]
    public string? PostalCode { get; set; }

    [StringLength(50, ErrorMessage = "Company name cannot exceed 50 characters")]
    public string? CompanyName { get; set; }

    [StringLength(50, ErrorMessage = "Title cannot exceed 50 characters")]
    public string? Title { get; set; }

    [StringLength(500, ErrorMessage = "Comments cannot exceed 500 characters")]
    public string? Comments { get; set; }
}
