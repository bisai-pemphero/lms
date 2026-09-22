using LandManagement.Api.DTOs.Report;
using Microsoft.EntityFrameworkCore;
using LandManagement.Api.Data;

namespace LandManagement.Api.Services;

/// <summary>
/// Service interface for report operations
/// </summary>
public interface IReportService
{
    Task<SalesReportResponse> GetSalesReportAsync(SalesReportRequest request);
    Task<ReminderResponse> GetRemindersAsync();
}

/// <summary>
/// Implementation of report service based on legacy SalesReport.aspx.cs and Reminder.aspx.cs
/// </summary>
public class ReportService : IReportService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ReportService> _logger;

    public ReportService(
        ApplicationDbContext context,
        ILogger<ReportService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<SalesReportResponse> GetSalesReportAsync(SalesReportRequest request)
    {
        var response = new SalesReportResponse();

        // Calculate statistics - Expected Income (sum of AgreedPrice for non-completed plots in district)
        if (!string.IsNullOrEmpty(request.Location))
        {
            response.Stats.ExpectedIncome = await _context.Plots
                .Where(p => p.SiteNo != null && p.Status != "Completed")
                .Join(_context.Sites.Where(s => s.District == request.Location),
                    p => p.SiteNo,
                    s => s.SiteCode,
                    (p, s) => p.AgreedPrice ?? 0)
                .SumAsync();

            response.Stats.CollectedAmount = await _context.PlotPayments
                .Where(p => p.SiteNo != null && p.DatePaid >= request.StartDate && p.DatePaid <= request.EndDate)
                .Join(_context.Sites.Where(s => s.District == request.Location),
                    p => p.SiteNo,
                    s => s.SiteCode,
                    (p, s) => p.AmountPaid ?? 0)
                .SumAsync();

            response.Stats.Balance = await _context.Plots
                .Where(p => p.SiteNo != null && p.Status != "Completed")
                .Join(_context.Sites.Where(s => s.District == request.Location),
                    p => p.SiteNo,
                    s => s.SiteCode,
                    (p, s) => p.Balance ?? 0)
                .SumAsync();

            // Calculate overdue amount - plots with offer period expired and balance > 0
            var now = DateTime.Now;
            response.Stats.OverdueAmount = await _context.Plots
                .Where(p => p.SiteNo != null && p.Status != "Completed" && p.Balance > 0)
                .Join(_context.Sites.Where(s => s.District == request.Location),
                    p => p.SiteNo,
                    s => s.SiteCode,
                    (p, s) => new { p, s })
                .Where(x => !string.IsNullOrEmpty(x.p.OfferPeriod) && int.TryParse(x.p.OfferPeriod, out int period) 
                    && x.p.DateOffered.AddMonths(period) < now)
                .SumAsync(x => x.p.Balance ?? 0);
        }

        // Get sales records within date range
        var salesQuery = from pp in _context.PlotPayments
                         join c in _context.Clients on pp.PaidBy equals c.ClientNo
                         join s in _context.Sites on pp.SiteNo equals s.SiteCode
                         where string.IsNullOrEmpty(request.Location) || s.District == request.Location
                         where pp.DatePaid >= request.StartDate && pp.DatePaid <= request.EndDate
                         orderby pp.DatePaid
                         select new SalesRecordDto
                         {
                             Fullname = c.Fullname,
                             PhysicalLocation = s.PhysicalLocation,
                             PlotNo = pp.PlotNo,
                             AmountPaid = pp.AmountPaid ?? 0,
                             NewBalance = pp.NewBalance ?? 0,
                             PaymentMode = pp.PaymentMode ?? string.Empty,
                             DatePaid = pp.DatePaid
                         };

        response.Records = await salesQuery.ToListAsync();

        // Get overdue plots
        var overdueQuery = from p in _context.Plots
                           join s in _context.Sites on p.SiteNo equals s.SiteCode
                           join c in _context.Clients on p.OfferedTo equals c.ClientNo
                           where string.IsNullOrEmpty(request.Location) || s.District == request.Location
                           where p.Status != "Completed" && p.Balance > 0
                           where !string.IsNullOrEmpty(p.OfferPeriod) && int.TryParse(p.OfferPeriod, out int period)
                           let dueDate = p.DateOffered.AddMonths(period)
                           where dueDate < DateTime.Now
                           select new OverduePlotDto
                           {
                               PhysicalLocation = s.PhysicalLocation,
                               PlotNo = p.PlotNo,
                               Fullname = c.Fullname,
                               DateOffered = p.DateOffered,
                               OfferPeriod = period,
                               AmountPaid = p.AmountPaid ?? 0,
                               Balance = p.Balance ?? 0,
                               DueDate = dueDate,
                               MonthsOverdue = (int)((DateTime.Now - dueDate).TotalDays / 30)
                           };

        response.OverduePlots = await overdueQuery.ToListAsync();

        _logger.LogInformation("Sales report generated for location: {Location}", request.Location);
        return response;
    }

    public async Task<ReminderResponse> GetRemindersAsync()
    {
        var now = DateTime.Now;

        // Based on Reminder.aspx.cs logic:
        // - OfferPeriod > 12 months
        // - Balance > 0
        // - PlotStatus != 'Pending'
        // - No payment made in current month
        var reminderQuery = from p in _context.Plots
                            join c in _context.Clients on p.OfferedTo equals c.ClientNo
                            join s in _context.Sites on p.SiteNo equals s.SiteCode
                            join pen in _context.Penalties on p.MonthlyInstallment equals 
                                (pen.FromRange <= p.MonthlyInstallment && pen.ToRange >= p.MonthlyInstallment ? pen.Charge : null)
                            where p.OfferPeriod != null && int.TryParse(p.OfferPeriod, out int period) && period > 12
                            where p.Balance > 0
                            where p.Status != "Pending"
                            where !_context.PlotPayments.Any(pp => 
                                pp.PlotNo == p.PlotNo && 
                                pp.SiteNo == p.SiteNo && 
                                pp.DatePaid.Month == now.Month && 
                                pp.DatePaid.Year == now.Year)
                            select new ReminderRecordDto
                            {
                                PlotNo = p.PlotNo,
                                PhysicalLocation = s.PhysicalLocation,
                                Fullname = c.Fullname,
                                PhoneNo = c.PhoneNo ?? string.Empty,
                                MonthlyInstallment = p.MonthlyInstallment ?? 0,
                                AgreedPrice = p.AgreedPrice ?? 0,
                                AmountPaid = p.AmountPaid ?? 0,
                                Balance = p.Balance ?? 0,
                                PenaltyCharge = pen.Charge ?? 0
                            };

        var records = await reminderQuery.ToListAsync();

        return new ReminderResponse
        {
            Records = records,
            TotalCount = records.Count
        };
    }
}
