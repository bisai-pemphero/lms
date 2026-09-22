namespace LandManagement.Api.DTOs.Client;

/// <summary>
/// DTO for client response matching legacy T_Clients table schema
/// </summary>
public class ClientResponse
{
    public string ClientNo { get; set; } = string.Empty;
    public string Fullname { get; set; } = string.Empty;
    public string? District { get; set; }
    public string? Address { get; set; }
    public string? PhoneNo { get; set; }
    public string? PhoneNo2 { get; set; }
    public string? Email { get; set; }
    public string? Occupation { get; set; }
    public string? IdentityNo { get; set; }
    public string? PostedBy { get; set; }
    
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
