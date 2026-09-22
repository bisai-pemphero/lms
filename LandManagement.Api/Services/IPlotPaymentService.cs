using LandManagement.Api.DTOs.PlotPayment;

namespace LandManagement.Api.Services;

public interface IPlotPaymentService
{
    Task<PlotPaymentResponse> RecordPaymentAsync(PlotPaymentRequest request, string postedBy);
    Task<PlotPaymentDto?> GetByIdAsync(int id);
    Task<IEnumerable<PlotPaymentDto>> GetByPlotNoAsync(string plotNo);
    Task<IEnumerable<PlotPaymentDto>> GetByClientNoAsync(string clientNo);
}
