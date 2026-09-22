namespace LandManagement.Api.DTOs.Document;

/// <summary>
/// Request model for generating offer letter
/// </summary>
public class OfferLetterRequest
{
    public string PlotNo { get; set; } = string.Empty;
}

/// <summary>
/// Response model for offer letter data
/// </summary>
public class OfferLetterDto
{
    public string ClientNo { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PlotNo { get; set; } = string.Empty;
    public string PlotSize { get; set; } = string.Empty;
    public string PhysicalLocation { get; set; } = string.Empty;
    public decimal AgreedPrice { get; set; }
    public DateTime DateOffered { get; set; }
    public string PriceCategory { get; set; } = string.Empty;
    public int OfferPeriod { get; set; }
    public string PriceInWords { get; set; } = string.Empty;
    public List<NextOfKinDto> NextOfKins { get; set; } = new();
}

/// <summary>
/// Model for next of kin information
/// </summary>
public class NextOfKinDto
{
    public string Name { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
    public string Contact { get; set; } = string.Empty;
}

/// <summary>
/// Request model for generating sale agreement document
/// </summary>
public class SaleAgreementDocumentRequest
{
    public string? District { get; set; }
    public string? SiteCode { get; set; }
    public int? SaleAgreementId { get; set; }
}

/// <summary>
/// Response model for sale agreement document data
/// </summary>
public class SaleAgreementDocumentDto
{
    public int Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string PhysicalLocation { get; set; } = string.Empty;
    public string PlotNumbers { get; set; } = string.Empty;
    public DateTime OfferDate { get; set; }
}

/// <summary>
/// Request model for generating change of ownership document
/// </summary>
public class ChangeOfOwnershipDocumentRequest
{
    public string ReceiptNo { get; set; } = string.Empty;
}

/// <summary>
/// Response model for change of ownership document data
/// </summary>
public class ChangeOfOwnershipDocumentDto
{
    public string SaleAgreementId { get; set; } = string.Empty;
    public string PreviousOwner { get; set; } = string.Empty;
    public string NewOwner { get; set; } = string.Empty;
    public string SiteName { get; set; } = string.Empty;
    public string PlotNumbers { get; set; } = string.Empty;
    public string PlotSize { get; set; } = string.Empty;
    public decimal Fee { get; set; }
    public string PaymentMode { get; set; } = string.Empty;
    public string ProofOfPayment { get; set; } = string.Empty;
    public string PreparedBy { get; set; } = string.Empty;
}
