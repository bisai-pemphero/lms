using LandManagement.Api.Data;
using LandManagement.Api.DTOs.PlotAllocation;
using LandManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace LandManagement.Api.Services;

public class PlotAllocationService : IPlotAllocationService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PlotAllocationService> _logger;

    public PlotAllocationService(ApplicationDbContext context, ILogger<PlotAllocationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PlotAllocationResponse> AllocatePlotAsync(PlotAllocationRequest request, string postedBy)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Verify client exists
            var client = await _context.Clients.FindAsync(request.ClientNo);
            if (client == null)
                return new PlotAllocationResponse { Success = false, Message = "Client not found" };

            // Verify plot exists and is available
            var plot = await _context.Plots.FindAsync(request.PlotNo);
            if (plot == null)
                return new PlotAllocationResponse { Success = false, Message = "Plot not found" };

            if (!string.IsNullOrEmpty(plot.Status) && plot.Status != "Available")
                return new PlotAllocationResponse { Success = false, Message = $"Plot is not available. Current status: {plot.Status}" };

            // Calculate values
            var agreedPrice = request.AgreedPrice > 0 ? request.AgreedPrice : plot.NormalPrice;
            var amountPaid = 0m;
            var balance = agreedPrice;

            // Create allocation
            var allocation = new PlotAllocation
            {
                ClientNo = request.ClientNo,
                PlotNo = request.PlotNo,
                AgreedPrice = agreedPrice,
                AmountPaid = amountPaid,
                Balance = balance,
                MonthlyInstallment = request.MonthlyInstallment,
                AllocationDate = request.AllocationDate ?? DateTime.Now,
                PostedBy = postedBy,
                CreatedOn = DateTime.Now
            };

            _context.PlotAllocations.Add(allocation);

            // Update plot status
            plot.Status = "Allocated";
            _context.Plots.Update(plot);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Plot {PlotNo} allocated to client {ClientNo} by {User}", request.PlotNo, request.ClientNo, postedBy);

            return new PlotAllocationResponse
            {
                Success = true,
                Message = "Plot allocated successfully",
                Data = new PlotAllocationDto
                {
                    Id = allocation.Id,
                    ClientNo = allocation.ClientNo,
                    ClientName = client.ClientName,
                    PlotNo = allocation.PlotNo,
                    SiteCode = plot.SiteNo,
                    AgreedPrice = allocation.AgreedPrice,
                    AmountPaid = allocation.AmountPaid,
                    Balance = allocation.Balance,
                    MonthlyInstallment = allocation.MonthlyInstallment,
                    AllocationDate = allocation.AllocationDate,
                    PostedBy = allocation.PostedBy,
                    Status = "Allocated"
                }
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error allocating plot");
            return new PlotAllocationResponse { Success = false, Message = $"Error: {ex.Message}" };
        }
    }

    public async Task<PlotAllocationDto?> GetByIdAsync(int id)
    {
        var allocation = await _context.PlotAllocations
            .Include(a => a.Client)
            .Include(a => a.Plot)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (allocation == null) return null;

        return new PlotAllocationDto
        {
            Id = allocation.Id,
            ClientNo = allocation.ClientNo,
            ClientName = allocation.Client?.ClientName,
            PlotNo = allocation.PlotNo,
            SiteCode = allocation.Plot?.SiteNo,
            AgreedPrice = allocation.AgreedPrice,
            AmountPaid = allocation.AmountPaid,
            Balance = allocation.Balance,
            MonthlyInstallment = allocation.MonthlyInstallment,
            AllocationDate = allocation.AllocationDate,
            PostedBy = allocation.PostedBy,
            Status = allocation.Plot?.Status ?? "Unknown"
        };
    }

    public async Task<IEnumerable<PlotAllocationDto>> GetAllAsync()
    {
        var allocations = await _context.PlotAllocations
            .Include(a => a.Client)
            .Include(a => a.Plot)
            .ToListAsync();

        return allocations.Select(a => new PlotAllocationDto
        {
            Id = a.Id,
            ClientNo = a.ClientNo,
            ClientName = a.Client?.ClientName,
            PlotNo = a.PlotNo,
            SiteCode = a.Plot?.SiteNo,
            AgreedPrice = a.AgreedPrice,
            AmountPaid = a.AmountPaid,
            Balance = a.Balance,
            MonthlyInstallment = a.MonthlyInstallment,
            AllocationDate = a.AllocationDate,
            PostedBy = a.PostedBy,
            Status = a.Plot?.Status ?? "Unknown"
        });
    }

    public async Task<IEnumerable<PlotAllocationDto>> GetByClientNoAsync(string clientNo)
    {
        var allocations = await _context.PlotAllocations
            .Include(a => a.Client)
            .Include(a => a.Plot)
            .Where(a => a.ClientNo == clientNo)
            .ToListAsync();

        return allocations.Select(a => new PlotAllocationDto
        {
            Id = a.Id,
            ClientNo = a.ClientNo,
            ClientName = a.Client?.ClientName,
            PlotNo = a.PlotNo,
            SiteCode = a.Plot?.SiteNo,
            AgreedPrice = a.AgreedPrice,
            AmountPaid = a.AmountPaid,
            Balance = a.Balance,
            MonthlyInstallment = a.MonthlyInstallment,
            AllocationDate = a.AllocationDate,
            PostedBy = a.PostedBy,
            Status = a.Plot?.Status ?? "Unknown"
        });
    }

    public async Task<IEnumerable<PlotAllocationDto>> GetByPlotNoAsync(string plotNo)
    {
        var allocations = await _context.PlotAllocations
            .Include(a => a.Client)
            .Include(a => a.Plot)
            .Where(a => a.PlotNo == plotNo)
            .ToListAsync();

        return allocations.Select(a => new PlotAllocationDto
        {
            Id = a.Id,
            ClientNo = a.ClientNo,
            ClientName = a.Client?.ClientName,
            PlotNo = a.PlotNo,
            SiteCode = a.Plot?.SiteNo,
            AgreedPrice = a.AgreedPrice,
            AmountPaid = a.AmountPaid,
            Balance = a.Balance,
            MonthlyInstallment = a.MonthlyInstallment,
            AllocationDate = a.AllocationDate,
            PostedBy = a.PostedBy,
            Status = a.Plot?.Status ?? "Unknown"
        });
    }
}
