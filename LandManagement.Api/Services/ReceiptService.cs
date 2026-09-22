using LandManagement.Api.DTOs.Receipt;
using Microsoft.EntityFrameworkCore;
using LandManagement.Api.Data;

namespace LandManagement.Api.Services;

/// <summary>
/// Service interface for receipt operations
/// </summary>
public interface IReceiptService
{
    Task<ReceiptDto?> GetReceiptByReceiptNoAsync(string receiptNo);
    Task<List<ReceiptDto>> GetReceiptsByPlotNoAsync(string plotNo);
    Task<List<ReceiptDto>> GetReceiptsByClientAsync(string clientNo);
    Task<List<ReceiptDto>> SearchReceiptsAsync(ReceiptQueryRequest request);
}

/// <summary>
/// Implementation of receipt service based on legacy Receipt.aspx.cs and ViewReceiptPayments.aspx.cs
/// </summary>
public class ReceiptService : IReceiptService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ReceiptService> _logger;

    public ReceiptService(
        ApplicationDbContext context,
        ILogger<ReceiptService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ReceiptDto?> GetReceiptByReceiptNoAsync(string receiptNo)
    {
        // Based on Receipt.aspx.cs LoadReceiptDetails()
        var query = from pp in _context.PlotPayments
                    join c in _context.Clients on pp.PaidBy equals c.ClientNo
                    join u in _context.Users on pp.PostedBy equals u.Username
                    join s in _context.Sites on pp.SiteNo equals s.SiteCode
                    join p in _context.Plots on pp.PlotNo equals p.PlotNo
                    where pp.ReceiptNo == receiptNo
                    select new ReceiptDto
                    {
                        ReceiptNo = pp.ReceiptNo ?? string.Empty,
                        PlotNo = pp.PlotNo ?? string.Empty,
                        AmountPaid = pp.AmountPaid ?? 0,
                        NewBalance = pp.NewBalance ?? 0,
                        CustomerName = c.Fullname ?? string.Empty,
                        PaymentMode = pp.PaymentMode ?? string.Empty,
                        DatePaid = pp.DatePaid,
                        PostedBy = u.Fullname ?? string.Empty,
                        Branch = u.Location != null ? u.Location + " Office" : string.Empty,
                        Site = s.PhysicalLocation ?? string.Empty,
                        PlotValue = p.AgreedPrice ?? 0,
                        TotalPaid = p.AmountPaid ?? 0,
                        CustomerAddress = c.Address ?? string.Empty
                    };

        var receipt = await query.FirstOrDefaultAsync();
        
        if (receipt != null)
        {
            _logger.LogInformation("Retrieved receipt: {ReceiptNo}", receiptNo);
        }
        
        return receipt;
    }

    public async Task<List<ReceiptDto>> GetReceiptsByPlotNoAsync(string plotNo)
    {
        var query = from pp in _context.PlotPayments
                    join c in _context.Clients on pp.PaidBy equals c.ClientNo
                    join u in _context.Users on pp.PostedBy equals u.Username
                    join s in _context.Sites on pp.SiteNo equals s.SiteCode
                    join p in _context.Plots on pp.PlotNo equals p.PlotNo
                    where pp.PlotNo == plotNo
                    orderby pp.DatePaid descending
                    select new ReceiptDto
                    {
                        ReceiptNo = pp.ReceiptNo ?? string.Empty,
                        PlotNo = pp.PlotNo ?? string.Empty,
                        AmountPaid = pp.AmountPaid ?? 0,
                        NewBalance = pp.NewBalance ?? 0,
                        CustomerName = c.Fullname ?? string.Empty,
                        PaymentMode = pp.PaymentMode ?? string.Empty,
                        DatePaid = pp.DatePaid,
                        PostedBy = u.Fullname ?? string.Empty,
                        Branch = u.Location != null ? u.Location + " Office" : string.Empty,
                        Site = s.PhysicalLocation ?? string.Empty,
                        PlotValue = p.AgreedPrice ?? 0,
                        TotalPaid = p.AmountPaid ?? 0,
                        CustomerAddress = c.Address ?? string.Empty
                    };

        var receipts = await query.ToListAsync();
        
        _logger.LogInformation("Retrieved {Count} receipts for plot: {PlotNo}", receipts.Count, plotNo);
        
        return receipts;
    }

    public async Task<List<ReceiptDto>> GetReceiptsByClientAsync(string clientNo)
    {
        var query = from pp in _context.PlotPayments
                    join c in _context.Clients on pp.PaidBy equals c.ClientNo
                    join u in _context.Users on pp.PostedBy equals u.Username
                    join s in _context.Sites on pp.SiteNo equals s.SiteCode
                    join p in _context.Plots on pp.PlotNo equals p.PlotNo
                    where pp.PaidBy == clientNo
                    orderby pp.DatePaid descending
                    select new ReceiptDto
                    {
                        ReceiptNo = pp.ReceiptNo ?? string.Empty,
                        PlotNo = pp.PlotNo ?? string.Empty,
                        AmountPaid = pp.AmountPaid ?? 0,
                        NewBalance = pp.NewBalance ?? 0,
                        CustomerName = c.Fullname ?? string.Empty,
                        PaymentMode = pp.PaymentMode ?? string.Empty,
                        DatePaid = pp.DatePaid,
                        PostedBy = u.Fullname ?? string.Empty,
                        Branch = u.Location != null ? u.Location + " Office" : string.Empty,
                        Site = s.PhysicalLocation ?? string.Empty,
                        PlotValue = p.AgreedPrice ?? 0,
                        TotalPaid = p.AmountPaid ?? 0,
                        CustomerAddress = c.Address ?? string.Empty
                    };

        var receipts = await query.ToListAsync();
        
        _logger.LogInformation("Retrieved {Count} receipts for client: {ClientNo}", receipts.Count, clientNo);
        
        return receipts;
    }

    public async Task<List<ReceiptDto>> SearchReceiptsAsync(ReceiptQueryRequest request)
    {
        var query = from pp in _context.PlotPayments
                    join c in _context.Clients on pp.PaidBy equals c.ClientNo
                    join u in _context.Users on pp.PostedBy equals u.Username
                    join s in _context.Sites on pp.SiteNo equals s.SiteCode
                    join p in _context.Plots on pp.PlotNo equals p.PlotNo
                    select new ReceiptDto
                    {
                        ReceiptNo = pp.ReceiptNo ?? string.Empty,
                        PlotNo = pp.PlotNo ?? string.Empty,
                        AmountPaid = pp.AmountPaid ?? 0,
                        NewBalance = pp.NewBalance ?? 0,
                        CustomerName = c.Fullname ?? string.Empty,
                        PaymentMode = pp.PaymentMode ?? string.Empty,
                        DatePaid = pp.DatePaid,
                        PostedBy = u.Fullname ?? string.Empty,
                        Branch = u.Location != null ? u.Location + " Office" : string.Empty,
                        Site = s.PhysicalLocation ?? string.Empty,
                        PlotValue = p.AgreedPrice ?? 0,
                        TotalPaid = p.AmountPaid ?? 0,
                        CustomerAddress = c.Address ?? string.Empty
                    };

        // Apply filters
        if (!string.IsNullOrEmpty(request.ReceiptNo))
        {
            query = query.Where(r => r.ReceiptNo.Contains(request.ReceiptNo));
        }

        if (!string.IsNullOrEmpty(request.PlotNo))
        {
            query = query.Where(r => r.PlotNo == request.PlotNo);
        }

        if (!string.IsNullOrEmpty(request.CustomerName))
        {
            query = query.Where(r => r.CustomerName.Contains(request.CustomerName));
        }

        if (request.StartDate.HasValue)
        {
            query = query.Where(r => r.DatePaid >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            query = query.Where(r => r.DatePaid <= request.EndDate.Value);
        }

        var receipts = await query.ToListAsync();
        
        _logger.LogInformation("Search returned {Count} receipts", receipts.Count);
        
        return receipts;
    }
}
