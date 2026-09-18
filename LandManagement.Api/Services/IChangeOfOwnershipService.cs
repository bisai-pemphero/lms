using LandManagement.Api.DTOs.ChangeOfOwnership;

namespace LandManagement.Api.Services;

public interface IChangeOfOwnershipService
{
    Task<ChangeOfOwnershipResponse> TransferOwnershipAsync(ChangeOfOwnershipRequest request, string postedBy);
    Task<ChangeOfOwnershipDto?> GetByIdAsync(int id);
    Task<IEnumerable<ChangeOfOwnershipDto>> GetAllAsync();
    Task<IEnumerable<ChangeOfOwnershipDto>> GetByPlotNoAsync(string plotNo);
}
