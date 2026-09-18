using LandManagement.Api.Data;
using LandManagement.Api.DTOs.Withdrawal;
using Microsoft.EntityFrameworkCore;

namespace LandManagement.Api.Services;

public class WithdrawalService : IWithdrawalService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<WithdrawalService> _logger;

    public WithdrawalService(ApplicationDbContext context, ILogger<WithdrawalService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<WithdrawalResponse> WithdrawAsync(WithdrawalRequest request, string postedBy)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var allocation = await _context.PlotAllocations
                .Include(a => a.Client)
                .FirstOrDefaultAsync(a => a.PlotNo == request.PlotNo);

            if (allocation == null)
                return new WithdrawalResponse { Success = false, Message = "Plot allocation not found" };

            var refundAmount = request.RefundAmount ?? allocation.AmountPaid;

            var withdrawal = new Entities.PlotWithdrawal
            {
                PlotNo = request.PlotNo,
                ClientNo = allocation.ClientNo,
                AmountPaid = allocation.AmountPaid,
                RefundAmount = refundAmount,
                Reason = request.Reason,
                WithdrawalDate = request.WithdrawalDate ?? DateTime.Now,
                Status = "Processed",
                PostedBy = postedBy
            };

            _context.PlotWithdrawals.Add(withdrawal);

            // Update plot status to Available
            var plot = await _context.Plots.FindAsync(request.PlotNo);
            if (plot != null)
            {
                plot.Status = "Available";
                _context.Plots.Update(plot);
            }

            // Remove or mark allocation as withdrawn
            _context.PlotAllocations.Remove(allocation);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Plot {PlotNo} withdrawn by {User}", request.PlotNo, postedBy);

            return new WithdrawalResponse
            {
                Success = true,
                Message = "Withdrawal processed successfully",
                Data = new WithdrawalDto
                {
                    Id = withdrawal.Id,
                    PlotNo = withdrawal.PlotNo,
                    ClientNo = withdrawal.ClientNo,
                    ClientName = allocation.Client?.ClientName,
                    AmountPaid = withdrawal.AmountPaid,
                    RefundAmount = withdrawal.RefundAmount,
                    Reason = withdrawal.Reason,
                    WithdrawalDate = withdrawal.WithdrawalDate,
                    Status = withdrawal.Status,
                    PostedBy = withdrawal.PostedBy
                }
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error processing withdrawal");
            return new WithdrawalResponse { Success = false, Message = $"Error: {ex.Message}" };
        }
    }

    public async Task<WithdrawalDto?> GetByIdAsync(int id)
    {
        var withdrawal = await _context.PlotWithdrawals.FindAsync(id);
        if (withdrawal == null) return null;

        var client = await _context.Clients.FindAsync(withdrawal.ClientNo);

        return new WithdrawalDto
        {
            Id = withdrawal.Id,
            PlotNo = withdrawal.PlotNo,
            ClientNo = withdrawal.ClientNo,
            ClientName = client?.ClientName,
            AmountPaid = withdrawal.AmountPaid,
            RefundAmount = withdrawal.RefundAmount,
            Reason = withdrawal.Reason,
            WithdrawalDate = withdrawal.WithdrawalDate,
            Status = withdrawal.Status,
            PostedBy = withdrawal.PostedBy
        };
    }

    public async Task<IEnumerable<WithdrawalDto>> GetAllAsync()
    {
        var withdrawals = await _context.PlotWithdrawals.ToListAsync();
        var result = new List<WithdrawalDto>();

        foreach (var w in withdrawals)
        {
            var client = await _context.Clients.FindAsync(w.ClientNo);
            result.Add(new WithdrawalDto
            {
                Id = w.Id,
                PlotNo = w.PlotNo,
                ClientNo = w.ClientNo,
                ClientName = client?.ClientName,
                AmountPaid = w.AmountPaid,
                RefundAmount = w.RefundAmount,
                Reason = w.Reason,
                WithdrawalDate = w.WithdrawalDate,
                Status = w.Status,
                PostedBy = w.PostedBy
            });
        }
        return result;
    }

    public async Task<IEnumerable<WithdrawalDto>> GetByPlotNoAsync(string plotNo)
    {
        var withdrawals = await _context.PlotWithdrawals.Where(w => w.PlotNo == plotNo).ToListAsync();
        var result = new List<WithdrawalDto>();

        foreach (var w in withdrawals)
        {
            var client = await _context.Clients.FindAsync(w.ClientNo);
            result.Add(new WithdrawalDto
            {
                Id = w.Id,
                PlotNo = w.PlotNo,
                ClientNo = w.ClientNo,
                ClientName = client?.ClientName,
                AmountPaid = w.AmountPaid,
                RefundAmount = w.RefundAmount,
                Reason = w.Reason,
                WithdrawalDate = w.WithdrawalDate,
                Status = w.Status,
                PostedBy = w.PostedBy
            });
        }
        return result;
    }
}
