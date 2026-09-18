namespace LandManagement.Api.DTOs.Client;

/// <summary>
/// DTO for client response with all relevant information
/// </summary>
public class ClientResponse
{
    public string ClientNo { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? PhysicalAddress { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? CompanyName { get; set; }
    public string? Title { get; set; }
    public string? Comments { get; set; }
    public string? Status { get; set; }
    public DateTime? DateCreated { get; set; }
    
    // Navigation properties (optional, can be populated separately)
    public List<NextOfKinResponse>? NextOfKins { get; set; }
    public int? TotalPlots { get; set; }
    public decimal? TotalAmountPaid { get; set; }
}

/// <summary>
/// Simplified next of kin response for nested client data
/// </summary>
public class NextOfKinResponse
{
    public int Id { get; set; }
    public string NextOfKinName { get; set; } = string.Empty;
    public string? Relationship { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? PhysicalAddress { get; set; }
}
