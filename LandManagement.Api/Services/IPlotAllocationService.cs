using LandManagement.Api.DTOs.PlotAllocation;

namespace LandManagement.Api.Services;

public interface IPlotAllocationService
{
    Task<PlotAllocationResponse> AllocatePlotAsync(PlotAllocationRequest request, string postedBy);
    Task<PlotAllocationDto?> GetByIdAsync(int id);
    Task<IEnumerable<PlotAllocationDto>> GetAllAsync();
    Task<IEnumerable<PlotAllocationDto>> GetByClientNoAsync(string clientNo);
    Task<IEnumerable<PlotAllocationDto>> GetByPlotNoAsync(string plotNo);
}
