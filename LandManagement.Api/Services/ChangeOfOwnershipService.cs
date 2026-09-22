using LandManagement.Api.Data;
using LandManagement.Api.DTOs.ChangeOfOwnership;
using Microsoft.EntityFrameworkCore;

namespace LandManagement.Api.Services;

public class ChangeOfOwnershipService : IChangeOfOwnershipService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ChangeOfOwnershipService> _logger;

    public ChangeOfOwnershipService(ApplicationDbContext context, ILogger<ChangeOfOwnershipService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ChangeOfOwnershipResponse> TransferOwnershipAsync(ChangeOfOwnershipRequest request, string postedBy)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var previousOwner = await _context.Clients.FindAsync(request.PreviousOwnerNo);
            if (previousOwner == null)
                return new ChangeOfOwnershipResponse { Success = false, Message = "Previous owner not found" };

            var currentOwner = await _context.Clients.FindAsync(request.CurrentOwnerNo);
            if (currentOwner == null)
                return new ChangeOfOwnershipResponse { Success = false, Message = "Current owner not found" };

            var allocation = await _context.PlotAllocations
                .FirstOrDefaultAsync(a => a.PlotNo == request.PlotNo && a.ClientNo == request.PreviousOwnerNo);
            
            if (allocation == null)
                return new ChangeOfOwnershipResponse { Success = false, Message = "Plot not found under previous owner" };

            var transferAmount = request.TransferAmount ?? allocation.Balance;

            var changeOfOwnership = new Entities.ChangeOfOwnership
            {
                PreviousOwnerNo = request.PreviousOwnerNo,
                CurrentOwnerNo = request.CurrentOwnerNo,
                PlotNo = request.PlotNo,
                TransferAmount = transferAmount,
                TransferDate = request.TransferDate ?? DateTime.Now,
                Status = "Completed",
                PostedBy = postedBy
            };

            _context.ChangeOfOwnerships.Add(changeOfOwnership);

            // Update allocation to new owner
            allocation.ClientNo = request.CurrentOwnerNo;
            _context.PlotAllocations.Update(allocation);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Ownership transferred for plot {PlotNo} from {Previous} to {Current}", 
                request.PlotNo, request.PreviousOwnerNo, request.CurrentOwnerNo);

            return new ChangeOfOwnershipResponse
            {
                Success = true,
                Message = "Ownership transferred successfully",
                Data = new ChangeOfOwnershipDto
                {
                    Id = changeOfOwnership.Id,
                    PreviousOwnerNo = changeOfOwnership.PreviousOwnerNo,
                    PreviousOwnerName = previousOwner.ClientName,
                    CurrentOwnerNo = changeOfOwnership.CurrentOwnerNo,
                    CurrentOwnerName = currentOwner.ClientName,
                    PlotNo = changeOfOwnership.PlotNo,
                    TransferAmount = changeOfOwnership.TransferAmount,
                    TransferDate = changeOfOwnership.TransferDate,
                    Status = changeOfOwnership.Status,
                    PostedBy = changeOfOwnership.PostedBy
                }
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error transferring ownership");
            return new ChangeOfOwnershipResponse { Success = false, Message = $"Error: {ex.Message}" };
        }
    }

    public async Task<ChangeOfOwnershipDto?> GetByIdAsync(int id)
    {
        var record = await _context.ChangeOfOwnerships.FindAsync(id);
        if (record == null) return null;

        var prevOwner = await _context.Clients.FindAsync(record.PreviousOwnerNo);
        var currOwner = await _context.Clients.FindAsync(record.CurrentOwnerNo);

        return new ChangeOfOwnershipDto
        {
            Id = record.Id,
            PreviousOwnerNo = record.PreviousOwnerNo,
            PreviousOwnerName = prevOwner?.ClientName,
            CurrentOwnerNo = record.CurrentOwnerNo,
            CurrentOwnerName = currOwner?.ClientName,
            PlotNo = record.PlotNo,
            TransferAmount = record.TransferAmount,
            TransferDate = record.TransferDate,
            Status = record.Status,
            PostedBy = record.PostedBy
        };
    }

    public async Task<IEnumerable<ChangeOfOwnershipDto>> GetAllAsync()
    {
        var records = await _context.ChangeOfOwnerships.ToListAsync();
        var result = new List<ChangeOfOwnershipDto>();

        foreach (var r in records)
        {
            var prev = await _context.Clients.FindAsync(r.PreviousOwnerNo);
            var curr = await _context.Clients.FindAsync(r.CurrentOwnerNo);
            result.Add(new ChangeOfOwnershipDto
            {
                Id = r.Id,
                PreviousOwnerNo = r.PreviousOwnerNo,
                PreviousOwnerName = prev?.ClientName,
                CurrentOwnerNo = r.CurrentOwnerNo,
                CurrentOwnerName = curr?.ClientName,
                PlotNo = r.PlotNo,
                TransferAmount = r.TransferAmount,
                TransferDate = r.TransferDate,
                Status = r.Status,
                PostedBy = r.PostedBy
            });
        }
        return result;
    }

    public async Task<IEnumerable<ChangeOfOwnershipDto>> GetByPlotNoAsync(string plotNo)
    {
        var records = await _context.ChangeOfOwnerships.Where(r => r.PlotNo == plotNo).ToListAsync();
        var result = new List<ChangeOfOwnershipDto>();

        foreach (var r in records)
        {
            var prev = await _context.Clients.FindAsync(r.PreviousOwnerNo);
            var curr = await _context.Clients.FindAsync(r.CurrentOwnerNo);
            result.Add(new ChangeOfOwnershipDto
            {
                Id = r.Id,
                PreviousOwnerNo = r.PreviousOwnerNo,
                PreviousOwnerName = prev?.ClientName,
                CurrentOwnerNo = r.CurrentOwnerNo,
                CurrentOwnerName = curr?.ClientName,
                PlotNo = r.PlotNo,
                TransferAmount = r.TransferAmount,
                TransferDate = r.TransferDate,
                Status = r.Status,
                PostedBy = r.PostedBy
            });
        }
        return result;
    }
}
