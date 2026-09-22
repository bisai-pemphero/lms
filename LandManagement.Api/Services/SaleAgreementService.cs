using LandManagement.Api.Data;
using LandManagement.Api.DTOs.SaleAgreement;
using LandManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace LandManagement.Api.Services;

public class SaleAgreementService : ISaleAgreementService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SaleAgreementService> _logger;

    public SaleAgreementService(ApplicationDbContext context, ILogger<SaleAgreementService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<SaleAgreementResponse> CreateAsync(SaleAgreementRequest request, string postedBy)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var client = await _context.Clients.FindAsync(request.ClientNo);
            if (client == null)
                return new SaleAgreementResponse { Success = false, Message = "Client not found" };

            if (request.PlotNos.Count == 0)
                return new SaleAgreementResponse { Success = false, Message = "At least one plot is required" };

            // Verify all plots exist and are available
            var plots = new List<Plot>();
            decimal totalAmount = 0;

            foreach (var plotNo in request.PlotNos)
            {
                var plot = await _context.Plots.FindAsync(plotNo);
                if (plot == null)
                    return new SaleAgreementResponse { Success = false, Message = $"Plot {plotNo} not found" };

                if (!string.IsNullOrEmpty(plot.Status) && plot.Status != "Available")
                    return new SaleAgreementResponse { Success = false, Message = $"Plot {plotNo} is not available" };

                plots.Add(plot);
                totalAmount += request.TotalAmount.HasValue 
                    ? request.TotalAmount.Value / request.PlotNos.Count 
                    : plot.NormalPrice;
            }

            // Generate agreement number
            var agreementNo = $"SA-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";

            var agreement = new Entities.SaleAgreement
            {
                AgreementNo = agreementNo,
                ClientNo = request.ClientNo,
                AgreementDate = request.AgreementDate.GetValueOrDefault(DateTime.Now),
                TotalAmount = request.TotalAmount ?? totalAmount,
                AmountPaid = 0,
                Balance = request.TotalAmount ?? totalAmount,
                Status = "Active",
                PostedBy = postedBy
            };

            _context.SaleAgreements.Add(agreement);
            await _context.SaveChangesAsync();

            // Add plot links
            foreach (var plot in plots)
            {
                var saPlot = new SaleAgreementPlot
                {
                    SaleAgreementId = agreement.Id,
                    PlotNo = plot.PlotNo,
                    AgreedPrice = request.TotalAmount.HasValue 
                        ? request.TotalAmount.Value / request.PlotNos.Count 
                        : plot.NormalPrice
                };
                _context.SaleAgreementPlots.Add(saPlot);

                // Update plot status
                plot.Status = "Sold";
                _context.Plots.Update(plot);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Sale Agreement {AgreementNo} created for client {ClientNo}", agreementNo, request.ClientNo);

            return new SaleAgreementResponse
            {
                Success = true,
                Message = "Sale agreement created successfully",
                Data = new SaleAgreementDto
                {
                    Id = agreement.Id,
                    AgreementNo = agreement.AgreementNo,
                    ClientNo = agreement.ClientNo,
                    ClientName = client.ClientName,
                    AgreementDate = agreement.AgreementDate ?? agreement.DateCreated ?? DateTime.Now,
                    TotalAmount = agreement.TotalAmount,
                    AmountPaid = agreement.AmountPaid,
                    Balance = agreement.Balance,
                    Status = agreement.Status,
                    PostedBy = agreement.PostedBy,
                    PlotNos = plots.Select(p => p.PlotNo).ToList()
                }
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error creating sale agreement");
            return new SaleAgreementResponse { Success = false, Message = $"Error: {ex.Message}" };
        }
    }

    public async Task<SaleAgreementDto?> GetByIdAsync(int id)
    {
        var agreement = await _context.SaleAgreements
            .Include(a => a.Client)
            .Include(a => a.SaleAgreementPlots)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (agreement == null) return null;

        return new SaleAgreementDto
        {
            Id = agreement.Id,
            AgreementNo = agreement.AgreementNo,
            ClientNo = agreement.ClientNo,
            ClientName = agreement.Client?.ClientName,
            AgreementDate = agreement.AgreementDate ?? agreement.DateCreated ?? DateTime.Now,
            TotalAmount = agreement.TotalAmount,
            AmountPaid = agreement.AmountPaid,
            Balance = agreement.Balance,
            Status = agreement.Status,
            PostedBy = agreement.PostedBy,
            PlotNos = agreement.SaleAgreementPlots.Select(p => p.PlotNo).ToList()
        };
    }

    public async Task<IEnumerable<SaleAgreementDto>> GetAllAsync()
    {
        var agreements = await _context.SaleAgreements
            .Include(a => a.Client)
            .Include(a => a.SaleAgreementPlots)
            .ToListAsync();

        return agreements.Select(a => new SaleAgreementDto
        {
            Id = a.Id,
            AgreementNo = a.AgreementNo,
            ClientNo = a.ClientNo,
            ClientName = a.Client?.ClientName,
            AgreementDate = a.AgreementDate,
            TotalAmount = a.TotalAmount,
            AmountPaid = a.AmountPaid,
            Balance = a.Balance,
            Status = a.Status,
            PostedBy = a.PostedBy,
            PlotNos = a.SaleAgreementPlots.Select(p => p.PlotNo).ToList()
        });
    }

    public async Task<IEnumerable<SaleAgreementDto>> GetByClientNoAsync(string clientNo)
    {
        var agreements = await _context.SaleAgreements
            .Include(a => a.Client)
            .Include(a => a.SaleAgreementPlots)
            .Where(a => a.ClientNo == clientNo)
            .ToListAsync();

        return agreements.Select(a => new SaleAgreementDto
        {
            Id = a.Id,
            AgreementNo = a.AgreementNo,
            ClientNo = a.ClientNo,
            ClientName = a.Client?.ClientName,
            AgreementDate = a.AgreementDate,
            TotalAmount = a.TotalAmount,
            AmountPaid = a.AmountPaid,
            Balance = a.Balance,
            Status = a.Status,
            PostedBy = a.PostedBy,
            PlotNos = a.SaleAgreementPlots.Select(p => p.PlotNo).ToList()
        });
    }
}
