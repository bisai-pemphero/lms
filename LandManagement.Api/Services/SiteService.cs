using LandManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;
using LandManagement.Api.Data;

namespace LandManagement.Api.Services;

/// <summary>
/// Service for handling site operations
/// </summary>
public interface ISiteService
{
    Task<List<Site>> GetAllAsync();
    Task<Site?> GetBySiteCodeAsync(string siteCode);
    Task<Site> CreateAsync(Site site);
    Task<Site?> UpdateAsync(string siteCode, Site siteUpdate);
    Task<bool> DeleteAsync(string siteCode);
    Task<List<Site>> SearchAsync(string searchTerm, int page = 1, int pageSize = 10);
    Task<int> GetTotalCountAsync();
}

/// <summary>
/// Implementation of site service
/// </summary>
public class SiteService : ISiteService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SiteService> _logger;

    public SiteService(
        ApplicationDbContext context,
        ILogger<SiteService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Site>> GetAllAsync()
    {
        return await _context.Sites
            .AsNoTracking()
            .OrderBy(s => s.SiteName)
            .ToListAsync();
    }

    public async Task<Site?> GetBySiteCodeAsync(string siteCode)
    {
        return await _context.Sites
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.SiteCode == siteCode);
    }

    public async Task<Site> CreateAsync(Site site)
    {
        site.DateCreated = DateTime.UtcNow;

        _context.Sites.Add(site);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Site created with SiteCode: {SiteCode}", site.SiteCode);

        return site;
    }

    public async Task<Site?> UpdateAsync(string siteCode, Site siteUpdate)
    {
        var existingSite = await _context.Sites.FindAsync(siteCode);
        
        if (existingSite == null)
            return null;

        // Update only allowed fields
        existingSite.SiteName = siteUpdate.SiteName;
        existingSite.District = siteUpdate.District;
        existingSite.PhysicalLocation = siteUpdate.PhysicalLocation;
        existingSite.Size = siteUpdate.Size;
        existingSite.PreviousOwner = siteUpdate.PreviousOwner;
        existingSite.InitialValue = siteUpdate.InitialValue;
        existingSite.AmountPaid = siteUpdate.AmountPaid;
        existingSite.Balance = siteUpdate.Balance;
        existingSite.DevelopmentsDone = siteUpdate.DevelopmentsDone;
        existingSite.DevelopmentCost = siteUpdate.DevelopmentCost;
        existingSite.TA = siteUpdate.TA;
        existingSite.Village = siteUpdate.Village;
        existingSite.Region = siteUpdate.Region;
        existingSite.Description = siteUpdate.Description;
        existingSite.PricePerSqMeter = siteUpdate.PricePerSqMeter;
        existingSite.BankName = siteUpdate.BankName;
        existingSite.AccountNumber = siteUpdate.AccountNumber;
        existingSite.RoadSize = siteUpdate.RoadSize;
        existingSite.RemainingAcreage = siteUpdate.RemainingAcreage;
        existingSite.SiteMap = siteUpdate.SiteMap;
        existingSite.Status = siteUpdate.Status ?? existingSite.Status;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Site updated: {SiteCode}", siteCode);

        return existingSite;
    }

    public async Task<bool> DeleteAsync(string siteCode)
    {
        var site = await _context.Sites.FindAsync(siteCode);
        
        if (site == null)
            return false;

        // Check for related plots before deletion
        var hasPlots = await _context.Plots.AnyAsync(p => p.SiteNo == siteCode);

        if (hasPlots)
        {
            throw new InvalidOperationException("Cannot delete site with existing plots");
        }

        _context.Sites.Remove(site);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Site deleted: {SiteCode}", siteCode);

        return true;
    }

    public async Task<List<Site>> SearchAsync(string searchTerm, int page = 1, int pageSize = 10)
    {
        var query = _context.Sites.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(s => 
                s.SiteName.Contains(searchTerm) ||
                s.SiteCode.Contains(searchTerm) ||
                s.District.Contains(searchTerm) ||
                s.PhysicalLocation.Contains(searchTerm));
        }

        return await query
            .AsNoTracking()
            .OrderBy(s => s.SiteName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Sites.CountAsync();
    }
}
