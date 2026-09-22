using LandManagement.Api.DTOs.SaleAgreement;

namespace LandManagement.Api.Services;

public interface ISaleAgreementService
{
    Task<SaleAgreementResponse> CreateAsync(SaleAgreementRequest request, string postedBy);
    Task<SaleAgreementDto?> GetByIdAsync(int id);
    Task<IEnumerable<SaleAgreementDto>> GetAllAsync();
    Task<IEnumerable<SaleAgreementDto>> GetByClientNoAsync(string clientNo);
}
