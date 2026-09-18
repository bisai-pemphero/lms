using LandManagement.Api.DTOs.Creditor;

namespace LandManagement.Api.Services;

public interface ICreditorService
{
    Task<CreditorResponse> CreateAsync(CreditorRequest request, string postedBy);
    Task<CreditorDto?> GetByIdAsync(int id);
    Task<IEnumerable<CreditorDto>> GetAllAsync();
}
