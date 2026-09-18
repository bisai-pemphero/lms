using LandManagement.Api.DTOs.Withdrawal;

namespace LandManagement.Api.Services;

public interface IWithdrawalService
{
    Task<WithdrawalResponse> WithdrawAsync(WithdrawalRequest request, string postedBy);
    Task<WithdrawalDto?> GetByIdAsync(int id);
    Task<IEnumerable<WithdrawalDto>> GetAllAsync();
    Task<IEnumerable<WithdrawalDto>> GetByPlotNoAsync(string plotNo);
}
