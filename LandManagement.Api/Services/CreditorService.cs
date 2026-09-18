using LandManagement.Api.Data;
using LandManagement.Api.DTOs.Creditor;
using Microsoft.EntityFrameworkCore;

namespace LandManagement.Api.Services;

public class CreditorService : ICreditorService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CreditorService> _logger;

    public CreditorService(ApplicationDbContext context, ILogger<CreditorService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CreditorResponse> CreateAsync(CreditorRequest request, string postedBy)
    {
        try
        {
            var creditor = new Entities.Creditor
            {
                CreditorName = request.CreditorName,
                SiteCode = request.SiteCode,
                AmountAgreed = request.AmountAgreed,
                AmountPaid = 0,
                Balance = request.AmountAgreed,
                Status = "Active",
                PostedBy = postedBy
            };

            _context.Creditors.Add(creditor);
            await _context.SaveChangesAsync();

            return new CreditorResponse
            {
                Success = true,
                Message = "Creditor created successfully",
                Data = new CreditorDto
                {
                    Id = creditor.Id,
                    CreditorName = creditor.CreditorName,
                    SiteCode = creditor.SiteCode,
                    AmountAgreed = creditor.AmountAgreed,
                    AmountPaid = creditor.AmountPaid,
                    Balance = creditor.Balance,
                    Status = creditor.Status,
                    PostedBy = creditor.PostedBy
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating creditor");
            return new CreditorResponse { Success = false, Message = $"Error: {ex.Message}" };
        }
    }

    public async Task<CreditorDto?> GetByIdAsync(int id)
    {
        var creditor = await _context.Creditors.FindAsync(id);
        if (creditor == null) return null;

        return new CreditorDto
        {
            Id = creditor.Id,
            CreditorName = creditor.CreditorName,
            SiteCode = creditor.SiteCode,
            AmountAgreed = creditor.AmountAgreed,
            AmountPaid = creditor.AmountPaid,
            Balance = creditor.Balance,
            Status = creditor.Status,
            PostedBy = creditor.PostedBy
        };
    }

    public async Task<IEnumerable<CreditorDto>> GetAllAsync()
    {
        var creditors = await _context.Creditors.ToListAsync();
        return creditors.Select(c => new CreditorDto
        {
            Id = c.Id,
            CreditorName = c.CreditorName,
            SiteCode = c.SiteCode,
            AmountAgreed = c.AmountAgreed,
            AmountPaid = c.AmountPaid,
            Balance = c.Balance,
            Status = c.Status,
            PostedBy = c.PostedBy
        });
    }
}
