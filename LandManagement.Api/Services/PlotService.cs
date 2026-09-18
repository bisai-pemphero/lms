using LandManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;
using LandManagement.Api.Data;

namespace LandManagement.Api.Services;

/// <summary>
/// Service for handling plot operations
/// </summary>
public interface IPlotService
{
    Task<List<Plot>> GetAllAsync();
    Task<Plot?> GetByPlotNoAsync(string plotNo);
    Task<Plot> CreateAsync(Plot plot);
    Task<Plot?> UpdateAsync(string plotNo, Plot plotUpdate);
    Task<bool> DeleteAsync(string plotNo);
    Task<List<Plot>> SearchAsync(string searchTerm, string? siteCode = null, string? status = null, int page = 1, int pageSize = 10);
    Task<int> GetTotalCountAsync();
    Task<List<Plot>> GetBySiteAsync(string siteCode);
    Task<List<Plot>> GetByStatusAsync(string status);
    Task<List<Plot>> GetAvailablePlotsAsync();
}

/// <summary>
/// Implementation of plot service
/// </summary>
public class PlotService : IPlotService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PlotService> _logger;

    public PlotService(
        ApplicationDbContext context,
        ILogger<PlotService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Plot>> GetAllAsync()
    {
        return await _context.Plots
            .AsNoTracking()
            .Include(p => p.Site)
            .OrderBy(p => p.PlotNo)
            .ToListAsync();
    }

    public async Task<Plot?> GetByPlotNoAsync(string plotNo)
    {
        return await _context.Plots
            .AsNoTracking()
            .Include(p => p.Site)
            .FirstOrDefaultAsync(p => p.PlotNo == plotNo);
    }

    public async Task<Plot> CreateAsync(Plot plot)
    {
        plot.DateCreated = DateTime.UtcNow;
        
        // Default status if not provided
        if (string.IsNullOrEmpty(plot.Status))
            plot.Status = "Available";

        _context.Plots.Add(plot);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Plot created with PlotNo: {PlotNo}", plot.PlotNo);

        return plot;
    }

    public async Task<Plot?> UpdateAsync(string plotNo, Plot plotUpdate)
    {
        var existingPlot = await _context.Plots.FindAsync(plotNo);
        
        if (existingPlot == null)
            return null;

        // Update only allowed fields
        existingPlot.Block = plotUpdate.Block;
        existingPlot.Area = plotUpdate.Area;
        existingPlot.Value = plotUpdate.Value;
        existingPlot.NormalPrice = plotUpdate.NormalPrice;
        existingPlot.PromotionPrice = plotUpdate.PromotionPrice;
        existingPlot.LandTitle = plotUpdate.LandTitle;
        existingPlot.Description = plotUpdate.Description;
        existingPlot.Status = plotUpdate.Status ?? existingPlot.Status;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Plot updated: {PlotNo}", plotNo);

        return existingPlot;
    }

    public async Task<bool> DeleteAsync(string plotNo)
    {
        var plot = await _context.Plots.FindAsync(plotNo);
        
        if (plot == null)
            return false;

        // Check for related records before deletion
        var hasAllocations = await _context.PlotAllocations.AnyAsync(a => a.PlotNo == plotNo);
        var hasPayments = await _context.PlotPayments.AnyAsync(p => p.PlotNo == plotNo);
        var hasSaleAgreementPlots = await _context.SaleAgreementPlots.AnyAsync(sap => sap.PlotNo == plotNo);

        if (hasAllocations || hasPayments || hasSaleAgreementPlots)
        {
            throw new InvalidOperationException("Cannot delete plot with existing allocations, payments, or sale agreements");
        }

        _context.Plots.Remove(plot);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Plot deleted: {PlotNo}", plotNo);

        return true;
    }

    public async Task<List<Plot>> SearchAsync(string searchTerm, string? siteCode = null, string? status = null, int page = 1, int pageSize = 10)
    {
        var query = _context.Plots.Include(p => p.Site).AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(p => 
                p.PlotNo.Contains(searchTerm) ||
                p.Block.Contains(searchTerm) ||
                p.Description.Contains(searchTerm));
        }

        if (!string.IsNullOrEmpty(siteCode))
        {
            query = query.Where(p => p.SiteNo == siteCode);
        }

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(p => p.Status == status);
        }

        return await query
            .AsNoTracking()
            .OrderBy(p => p.PlotNo)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Plots.CountAsync();
    }

    public async Task<List<Plot>> GetBySiteAsync(string siteCode)
    {
        return await _context.Plots
            .AsNoTracking()
            .Where(p => p.SiteNo == siteCode)
            .OrderBy(p => p.PlotNo)
            .ToListAsync();
    }

    public async Task<List<Plot>> GetByStatusAsync(string status)
    {
        return await _context.Plots
            .AsNoTracking()
            .Where(p => p.Status == status)
            .OrderBy(p => p.PlotNo)
            .ToListAsync();
    }

    public async Task<List<Plot>> GetAvailablePlotsAsync()
    {
        return await _context.Plots
            .AsNoTracking()
            .Where(p => p.Status == "Available")
            .OrderBy(p => p.PlotNo)
            .ToListAsync();
    }
}
