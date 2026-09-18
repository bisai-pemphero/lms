using LandManagement.Api.Data;
using LandManagement.Api.DTOs.Dashboard;
using Microsoft.EntityFrameworkCore;

namespace LandManagement.Api.Services;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context) => _context = context;

    public async Task<DashboardSummaryDto> GetSummaryAsync()
    {
        var totalSites = await _context.Sites.CountAsync();
        var totalPlots = await _context.Plots.CountAsync();
        var availablePlots = await _context.Plots.CountAsync(p => p.Status == "Available");
        var allocatedPlots = await _context.Plots.CountAsync(p => p.Status == "Allocated");
        var fullyPaidPlots = await _context.Plots.CountAsync(p => p.Status == "Fully Paid");
        var withdrawnPlots = await _context.PlotWithdrawals.CountAsync();
        
        var totalSales = await _context.PlotAllocations.SumAsync(a => (decimal?)a.AgreedPrice) ?? 0;
        var totalCollected = await _context.PlotPayments.SumAsync(p => (decimal?)p.AmountPaid) ?? 0;
        var outstandingBalance = await _context.PlotAllocations.SumAsync(a => (decimal?)a.Balance) ?? 0;
        
        var totalClients = await _context.Clients.CountAsync();
        var totalCreditors = await _context.Creditors.CountAsync();

        return new DashboardSummaryDto
        {
            TotalSites = totalSites,
            TotalPlots = totalPlots,
            AvailablePlots = availablePlots,
            AllocatedPlots = allocatedPlots,
            FullyPaidPlots = fullyPaidPlots,
            WithdrawnPlots = withdrawnPlots,
            TotalSales = totalSales,
            TotalCollected = totalCollected,
            OutstandingBalance = outstandingBalance,
            TotalClients = totalClients,
            TotalCreditors = totalCreditors
        };
    }

    public async Task<IEnumerable<SitePerformanceDto>> GetSitePerformanceAsync()
    {
        var sites = await _context.Sites.ToListAsync();
        var result = new List<SitePerformanceDto>();

        foreach (var site in sites)
        {
            var plots = await _context.Plots.Where(p => p.SiteNo == site.SiteCode).ToListAsync();
            result.Add(new SitePerformanceDto
            {
                SiteCode = site.SiteCode,
                SiteName = site.SiteName,
                TotalPlots = plots.Count,
                AvailablePlots = plots.Count(p => p.Status == "Available"),
                SoldPlots = plots.Count(p => p.Status is "Allocated" or "Fully Paid" or "Sold"),
                TotalValue = plots.Sum(p => p.NormalPrice),
                Collected = _context.PlotAllocations
                    .Where(a => a.PlotNo.StartsWith(site.SiteCode))
                    .Sum(a => (decimal?)a.AmountPaid) ?? 0
            });
        }

        return result;
    }

    public async Task<IEnumerable<RecentTransactionDto>> GetRecentTransactionsAsync(int count = 10)
    {
        var payments = await _context.PlotPayments
            .OrderByDescending(p => p.DatePaid)
            .Take(count)
            .Select(p => new RecentTransactionDto
            {
                Type = "Payment",
                Reference = p.ReceiptNumber ?? $"PMT-{p.Id}",
                Amount = p.AmountPaid,
                Date = p.DatePaid,
                Description = $"Payment for plot {p.PlotNo}"
            })
            .ToListAsync();

        var allocations = await _context.PlotAllocations
            .OrderByDescending(a => a.AllocationDate)
            .Take(count)
            .Select(a => new RecentTransactionDto
            {
                Type = "Allocation",
                Reference = $"ALL-{a.Id}",
                Amount = a.AgreedPrice,
                Date = a.AllocationDate,
                Description = $"Plot {a.PlotNo} allocated"
            })
            .ToListAsync();

        return payments.Concat(allocations)
            .OrderByDescending(t => t.Date)
            .Take(count);
    }
}

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetSummaryAsync();
    Task<IEnumerable<SitePerformanceDto>> GetSitePerformanceAsync();
    Task<IEnumerable<RecentTransactionDto>> GetRecentTransactionsAsync(int count = 10);
}
