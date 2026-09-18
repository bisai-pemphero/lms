using LandManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;
using LandManagement.Api.Data;

namespace LandManagement.Api.Services;

/// <summary>
/// Service for handling client operations
/// </summary>
public interface IClientService
{
    Task<List<Client>> GetAllAsync();
    Task<Client?> GetByClientNoAsync(string clientNo);
    Task<Client> CreateAsync(Client client);
    Task<Client?> UpdateAsync(string clientNo, Client client);
    Task<bool> DeleteAsync(string clientNo);
    Task<List<Client>> SearchAsync(string searchTerm, int page = 1, int pageSize = 10);
    Task<int> GetTotalCountAsync();
}

/// <summary>
/// Implementation of client service
/// </summary>
public class ClientService : IClientService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ClientService> _logger;

    public ClientService(
        ApplicationDbContext context,
        ILogger<ClientService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Client>> GetAllAsync()
    {
        return await _context.Clients
            .AsNoTracking()
            .OrderBy(c => c.ClientName)
            .ToListAsync();
    }

    public async Task<Client?> GetByClientNoAsync(string clientNo)
    {
        return await _context.Clients
            .AsNoTracking()
            .Include(c => c.NextOfKins)
            .FirstOrDefaultAsync(c => c.ClientNo == clientNo);
    }

    public async Task<Client> CreateAsync(Client client)
    {
        // Generate client number if not provided
        if (string.IsNullOrEmpty(client.ClientNo))
        {
            client.ClientNo = await GenerateClientNoAsync();
        }

        client.DateCreated = DateTime.UtcNow;

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Client created with ClientNo: {ClientNo}", client.ClientNo);

        return client;
    }

    public async Task<Client?> UpdateAsync(string clientNo, Client clientUpdate)
    {
        var existingClient = await _context.Clients.FindAsync(clientNo);
        
        if (existingClient == null)
            return null;

        // Update only allowed fields
        existingClient.ClientName = clientUpdate.ClientName;
        existingClient.Email = clientUpdate.Email;
        existingClient.PhoneNumber = clientUpdate.PhoneNumber;
        existingClient.PhysicalAddress = clientUpdate.PhysicalAddress;
        existingClient.City = clientUpdate.City;
        existingClient.Country = clientUpdate.Country;
        existingClient.PostalCode = clientUpdate.PostalCode;
        existingClient.CompanyName = clientUpdate.CompanyName;
        existingClient.Title = clientUpdate.Title;
        existingClient.Comments = clientUpdate.Comments;
        existingClient.Status = clientUpdate.Status ?? existingClient.Status;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Client updated: {ClientNo}", clientNo);

        return existingClient;
    }

    public async Task<bool> DeleteAsync(string clientNo)
    {
        var client = await _context.Clients.FindAsync(clientNo);
        
        if (client == null)
            return false;

        // Check for related records before deletion
        var hasAllocations = await _context.PlotAllocations.AnyAsync(a => a.ClientNo == clientNo);
        var hasSaleAgreements = await _context.SaleAgreements.AnyAsync(s => s.ClientNo == clientNo);

        if (hasAllocations || hasSaleAgreements)
        {
            throw new InvalidOperationException("Cannot delete client with existing plot allocations or sale agreements");
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Client deleted: {ClientNo}", clientNo);

        return true;
    }

    public async Task<List<Client>> SearchAsync(string searchTerm, int page = 1, int pageSize = 10)
    {
        var query = _context.Clients.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(c => 
                c.ClientName.Contains(searchTerm) ||
                c.ClientNo.Contains(searchTerm) ||
                c.Email.Contains(searchTerm) ||
                c.PhoneNumber.Contains(searchTerm));
        }

        return await query
            .AsNoTracking()
            .OrderBy(c => c.ClientName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Clients.CountAsync();
    }

    /// <summary>
    /// Generates a unique client number using the ValueSequence table
    /// </summary>
    private async Task<string> GenerateClientNoAsync()
    {
        // Try to get sequence from ValueSequence table
        var sequence = await _context.ValueSequences
            .FirstOrDefaultAsync(vs => vs.SequenceName == "CLIENTNO");

        if (sequence != null)
        {
            var nextValue = (sequence.LastValue ?? 0) + 1;
            sequence.LastValue = nextValue;
            await _context.SaveChangesAsync();
            
            // Format: INC + padded number (e.g., INC00001)
            return $"INC{nextValue:D5}";
        }

        // Fallback: Generate based on current max
        var maxClient = await _context.Clients
            .Where(c => c.ClientNo.StartsWith("INC"))
            .OrderByDescending(c => c.ClientNo)
            .FirstOrDefaultAsync();

        if (maxClient != null && int.TryParse(maxClient.ClientNo.Substring(3), out var lastNumber))
        {
            return $"INC{(lastNumber + 1):D5}";
        }

        return "INC00001";
    }
}
