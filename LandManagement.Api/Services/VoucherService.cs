using LandManagement.Api.DTOs.Voucher;
using Microsoft.EntityFrameworkCore;
using LandManagement.Api.Data;

namespace LandManagement.Api.Services;

/// <summary>
/// Service interface for voucher operations
/// </summary>
public interface IVoucherService
{
    Task<List<VoucherDto>> GetPendingVouchersAsync(bool isCreditor = false);
    Task<VoucherDto?> GetVoucherByIdAsync(int voucherId, bool isCreditor = false);
    Task<VoucherDto> CreateVoucherAsync(CreateVoucherRequest request, string createdBy);
    Task<VoucherDto?> ApproveVoucherAsync(int voucherId, string approvedBy, bool isCreditor = false);
}

/// <summary>
/// Implementation of voucher service based on legacy Vouchers.aspx.cs and VoucherCreditor.aspx.cs
/// </summary>
public class VoucherService : IVoucherService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<VoucherService> _logger;

    public VoucherService(
        ApplicationDbContext context,
        ILogger<VoucherService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<VoucherDto>> GetPendingVouchersAsync(bool isCreditor = false)
    {
        if (isCreditor)
        {
            // From VoucherCreditor.aspx.cs - PaymentVoucherCreditor table
            var vouchers = await _context.PaymentVoucherCreditors
                .Where(v => v.Status == "Pending")
                .Select(v => new VoucherDto
                {
                    PaymentVoucherId = v.PaymentVoucherId,
                    PayeeName = v.PayeeName ?? string.Empty,
                    Description = v.Description ?? string.Empty,
                    Amount = v.Amount ?? 0,
                    Status = v.Status ?? "Pending",
                    CreatedDate = v.CreatedDate ?? DateTime.Now
                })
                .ToListAsync();

            return vouchers;
        }
        else
        {
            // From Vouchers.aspx.cs - PaymentVoucher table
            var vouchers = await _context.PaymentVouchers
                .Where(v => v.Status == "Pending")
                .Select(v => new VoucherDto
                {
                    PaymentVoucherId = v.PaymentVoucherId,
                    PayeeName = v.PayeeName ?? string.Empty,
                    Description = v.Description ?? string.Empty,
                    Amount = v.Amount ?? 0,
                    Status = v.Status ?? "Pending",
                    CreatedDate = v.CreatedDate ?? DateTime.Now
                })
                .ToListAsync();

            return vouchers;
        }
    }

    public async Task<VoucherDto?> GetVoucherByIdAsync(int voucherId, bool isCreditor = false)
    {
        if (isCreditor)
        {
            var voucher = await _context.PaymentVoucherCreditors
                .FindAsync(voucherId);

            if (voucher == null) return null;

            return new VoucherDto
            {
                PaymentVoucherId = voucher.PaymentVoucherId,
                PayeeName = voucher.PayeeName ?? string.Empty,
                Description = voucher.Description ?? string.Empty,
                Amount = voucher.Amount ?? 0,
                Status = voucher.Status ?? string.Empty,
                CreatedDate = voucher.CreatedDate ?? DateTime.Now,
                ApprovedBy = voucher.ApprovedBy,
                ApprovedDate = voucher.ApprovedDate
            };
        }
        else
        {
            var voucher = await _context.PaymentVouchers
                .FindAsync(voucherId);

            if (voucher == null) return null;

            return new VoucherDto
            {
                PaymentVoucherId = voucher.PaymentVoucherId,
                PayeeName = voucher.PayeeName ?? string.Empty,
                Description = voucher.Description ?? string.Empty,
                Amount = voucher.Amount ?? 0,
                Status = voucher.Status ?? string.Empty,
                CreatedDate = voucher.CreatedDate ?? DateTime.Now,
                ApprovedBy = voucher.ApprovedBy,
                ApprovedDate = voucher.ApprovedDate
            };
        }
    }

    public async Task<VoucherDto> CreateVoucherAsync(CreateVoucherRequest request, string createdBy)
    {
        if (request.IsCreditor)
        {
            var voucher = new PaymentVoucherCreditor
            {
                PayeeName = request.PayeeName,
                Description = request.Description,
                Amount = request.Amount,
                Status = "Pending",
                CreatedDate = DateTime.Now,
                CreatedBy = createdBy
            };

            _context.PaymentVoucherCreditors.Add(voucher);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Creditor voucher created: {VoucherId}", voucher.PaymentVoucherId);

            return new VoucherDto
            {
                PaymentVoucherId = voucher.PaymentVoucherId,
                PayeeName = voucher.PayeeName,
                Description = voucher.Description,
                Amount = voucher.Amount ?? 0,
                Status = voucher.Status ?? "Pending",
                CreatedDate = voucher.CreatedDate ?? DateTime.Now
            };
        }
        else
        {
            var voucher = new PaymentVoucher
            {
                PayeeName = request.PayeeName,
                Description = request.Description,
                Amount = request.Amount,
                Status = "Pending",
                CreatedDate = DateTime.Now,
                CreatedBy = createdBy
            };

            _context.PaymentVouchers.Add(voucher);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Voucher created: {VoucherId}", voucher.PaymentVoucherId);

            return new VoucherDto
            {
                PaymentVoucherId = voucher.PaymentVoucherId,
                PayeeName = voucher.PayeeName,
                Description = voucher.Description,
                Amount = voucher.Amount ?? 0,
                Status = voucher.Status ?? "Pending",
                CreatedDate = voucher.CreatedDate ?? DateTime.Now
            };
        }
    }

    public async Task<VoucherDto?> ApproveVoucherAsync(int voucherId, string approvedBy, bool isCreditor = false)
    {
        if (isCreditor)
        {
            var voucher = await _context.PaymentVoucherCreditors.FindAsync(voucherId);
            if (voucher == null) return null;

            voucher.Status = "Approved";
            voucher.ApprovedBy = approvedBy;
            voucher.ApprovedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Creditor voucher approved: {VoucherId}", voucherId);

            return new VoucherDto
            {
                PaymentVoucherId = voucher.PaymentVoucherId,
                PayeeName = voucher.PayeeName ?? string.Empty,
                Description = voucher.Description ?? string.Empty,
                Amount = voucher.Amount ?? 0,
                Status = voucher.Status ?? "Approved",
                CreatedDate = voucher.CreatedDate ?? DateTime.Now,
                ApprovedBy = voucher.ApprovedBy,
                ApprovedDate = voucher.ApprovedDate
            };
        }
        else
        {
            var voucher = await _context.PaymentVouchers.FindAsync(voucherId);
            if (voucher == null) return null;

            voucher.Status = "Approved";
            voucher.ApprovedBy = approvedBy;
            voucher.ApprovedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Voucher approved: {VoucherId}", voucherId);

            return new VoucherDto
            {
                PaymentVoucherId = voucher.PaymentVoucherId,
                PayeeName = voucher.PayeeName ?? string.Empty,
                Description = voucher.Description ?? string.Empty,
                Amount = voucher.Amount ?? 0,
                Status = voucher.Status ?? "Approved",
                CreatedDate = voucher.CreatedDate ?? DateTime.Now,
                ApprovedBy = voucher.ApprovedBy,
                ApprovedDate = voucher.ApprovedDate
            };
        }
    }
}
