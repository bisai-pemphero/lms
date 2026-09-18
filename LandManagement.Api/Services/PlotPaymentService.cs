using LandManagement.Api.Data;
using LandManagement.Api.DTOs.PlotPayment;
using LandManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace LandManagement.Api.Services;

public class PlotPaymentService : IPlotPaymentService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PlotPaymentService> _logger;

    public PlotPaymentService(ApplicationDbContext context, ILogger<PlotPaymentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PlotPaymentResponse> RecordPaymentAsync(PlotPaymentRequest request, string postedBy)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Find allocation by plot number
            var allocation = await _context.PlotAllocations
                .Include(a => a.Client)
                .FirstOrDefaultAsync(a => a.PlotNo == request.PlotNo);

            if (allocation == null)
                return new PlotPaymentResponse { Success = false, Message = "No allocation found for this plot" };

            // Validate amount
            if (request.AmountPaid <= 0)
                return new PlotPaymentResponse { Success = false, Message = "Payment amount must be greater than 0" };

            if (request.AmountPaid > allocation.Balance)
                return new PlotPaymentResponse { Success = false, Message = $"Payment amount exceeds outstanding balance of {allocation.Balance}" };

            // Calculate new balance
            var newBalance = allocation.Balance - request.AmountPaid;
            var newAmountPaid = allocation.AmountPaid + request.AmountPaid;

            // Generate receipt number
            var receiptNumber = $"RCP-{DateTime.Now:yyyyMMddHHmmss}-{allocation.PlotNo}";

            // Create payment record
            var payment = new PlotPayment
            {
                PlotNo = request.PlotNo,
                AmountPaid = request.AmountPaid,
                Balance = newBalance,
                DatePaid = request.DatePaid ?? DateTime.Now,
                PaymentMode = request.PaymentMode ?? "Cash",
                ReceiptNumber = receiptNumber,
                PostedBy = postedBy
            };

            _context.PlotPayments.Add(payment);

            // Update allocation
            allocation.AmountPaid = newAmountPaid;
            allocation.Balance = newBalance;

            // Update plot status if fully paid
            if (newBalance <= 0)
            {
                var plot = await _context.Plots.FindAsync(request.PlotNo);
                if (plot != null)
                {
                    plot.Status = "Fully Paid";
                    _context.Plots.Update(plot);
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Payment of {Amount} recorded for plot {PlotNo}. Receipt: {Receipt}", 
                request.AmountPaid, request.PlotNo, receiptNumber);

            return new PlotPaymentResponse
            {
                Success = true,
                Message = "Payment recorded successfully",
                Data = new PlotPaymentDto
                {
                    Id = payment.Id,
                    PlotNo = payment.PlotNo,
                    ClientNo = allocation.ClientNo,
                    ClientName = allocation.Client?.ClientName,
                    AmountPaid = payment.AmountPaid,
                    Balance = payment.Balance,
                    DatePaid = payment.DatePaid,
                    PaymentMode = payment.PaymentMode,
                    ReceiptNumber = payment.ReceiptNumber,
                    PostedBy = payment.PostedBy
                }
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error recording payment");
            return new PlotPaymentResponse { Success = false, Message = $"Error: {ex.Message}" };
        }
    }

    public async Task<PlotPaymentDto?> GetByIdAsync(int id)
    {
        var payment = await _context.PlotPayments
            .Include(p => p.Plot)
            .ThenInclude(p => p!.PlotAllocations)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (payment == null) return null;

        var clientNo = payment.Plot?.PlotAllocations.FirstOrDefault()?.ClientNo;
        var client = clientNo != null ? await _context.Clients.FindAsync(clientNo) : null;

        return new PlotPaymentDto
        {
            Id = payment.Id,
            PlotNo = payment.PlotNo,
            ClientNo = clientNo,
            ClientName = client?.ClientName,
            AmountPaid = payment.AmountPaid,
            Balance = payment.Balance,
            DatePaid = payment.DatePaid,
            PaymentMode = payment.PaymentMode,
            ReceiptNumber = payment.ReceiptNumber,
            PostedBy = payment.PostedBy
        };
    }

    public async Task<IEnumerable<PlotPaymentDto>> GetByPlotNoAsync(string plotNo)
    {
        var payments = await _context.PlotPayments
            .Where(p => p.PlotNo == plotNo)
            .OrderByDescending(p => p.DatePaid)
            .ToListAsync();

        var allocation = await _context.PlotAllocations.FirstOrDefaultAsync(a => a.PlotNo == plotNo);
        var client = allocation != null ? await _context.Clients.FindAsync(allocation.ClientNo) : null;

        return payments.Select(p => new PlotPaymentDto
        {
            Id = p.Id,
            PlotNo = p.PlotNo,
            ClientNo = allocation?.ClientNo,
            ClientName = client?.ClientName,
            AmountPaid = p.AmountPaid,
            Balance = p.Balance,
            DatePaid = p.DatePaid,
            PaymentMode = p.PaymentMode,
            ReceiptNumber = p.ReceiptNumber,
            PostedBy = p.PostedBy
        });
    }

    public async Task<IEnumerable<PlotPaymentDto>> GetByClientNoAsync(string clientNo)
    {
        var allocations = await _context.PlotAllocations.Where(a => a.ClientNo == clientNo).ToListAsync();
        var plotNos = allocations.Select(a => a.PlotNo).ToList();
        
        var payments = await _context.PlotPayments
            .Where(p => plotNos.Contains(p.PlotNo))
            .OrderByDescending(p => p.DatePaid)
            .ToListAsync();

        var client = await _context.Clients.FindAsync(clientNo);

        return payments.Select(p => new PlotPaymentDto
        {
            Id = p.Id,
            PlotNo = p.PlotNo,
            ClientNo = clientNo,
            ClientName = client?.ClientName,
            AmountPaid = p.AmountPaid,
            Balance = p.Balance,
            DatePaid = p.DatePaid,
            PaymentMode = p.PaymentMode,
            ReceiptNumber = p.ReceiptNumber,
            PostedBy = p.PostedBy
        });
    }
}
