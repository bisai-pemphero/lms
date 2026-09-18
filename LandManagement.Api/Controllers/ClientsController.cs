using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LandManagement.Api.DTOs.Client;
using LandManagement.Api.Entities;
using LandManagement.Api.Services;
using LandManagement.Api.Shared.Models;

namespace LandManagement.Api.Controllers;

/// <summary>
/// Controller for client operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly ILogger<ClientsController> _logger;

    public ClientsController(IClientService clientService, ILogger<ClientsController> logger)
    {
        _clientService = clientService;
        _logger = logger;
    }

    /// <summary>
    /// Get all clients with optional pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<ClientResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<ClientResponse>>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var clients = await _clientService.GetAllAsync();
        
        var response = clients.Select(c => new ClientResponse
        {
            ClientNo = c.ClientNo,
            ClientName = c.ClientName,
            Email = c.Email,
            PhoneNumber = c.PhoneNumber,
            PhysicalAddress = c.PhysicalAddress,
            City = c.City,
            Country = c.Country,
            PostalCode = c.PostalCode,
            CompanyName = c.CompanyName,
            Title = c.Title,
            Comments = c.Comments,
            Status = c.Status,
            DateCreated = c.DateCreated
        }).ToList();

        return Ok(ApiResponse.Ok(response));
    }

    /// <summary>
    /// Get client by client number
    /// </summary>
    [HttpGet("{clientNo}")]
    [ProducesResponseType(typeof(ApiResponse<ClientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ClientResponse>>> GetByClientNo(string clientNo)
    {
        var client = await _clientService.GetByClientNoAsync(clientNo);

        if (client == null)
            return NotFound(ApiResponse<ClientResponse>.Fail($"Client with number {clientNo} not found"));

        var response = new ClientResponse
        {
            ClientNo = client.ClientNo,
            ClientName = client.ClientName,
            Email = client.Email,
            PhoneNumber = client.PhoneNumber,
            PhysicalAddress = client.PhysicalAddress,
            City = client.City,
            Country = client.Country,
            PostalCode = client.PostalCode,
            CompanyName = client.CompanyName,
            Title = client.Title,
            Comments = client.Comments,
            Status = client.Status,
            DateCreated = client.DateCreated,
            NextOfKins = client.NextOfKins?.Select(n => new NextOfKinResponse
            {
                Id = n.Id,
                NextOfKinName = n.NextOfKinName,
                Relationship = n.Relationship,
                PhoneNumber = n.PhoneNumber,
                Email = n.Email,
                PhysicalAddress = n.PhysicalAddress
            }).ToList()
        };

        return Ok(ApiResponse.Ok(response));
    }

    /// <summary>
    /// Create a new client
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ClientResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<ClientResponse>>> Create([FromBody] ClientCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<ClientResponse>.Fail("Invalid client data", GetValidationErrors()));

        var client = new Client
        {
            ClientName = request.ClientName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            PhysicalAddress = request.PhysicalAddress,
            City = request.City,
            Country = request.Country,
            PostalCode = request.PostalCode,
            CompanyName = request.CompanyName,
            Title = request.Title,
            Comments = request.Comments
        };

        var createdClient = await _clientService.CreateAsync(client);

        var response = new ClientResponse
        {
            ClientNo = createdClient.ClientNo,
            ClientName = createdClient.ClientName,
            Email = createdClient.Email,
            PhoneNumber = createdClient.PhoneNumber,
            PhysicalAddress = createdClient.PhysicalAddress,
            City = createdClient.City,
            Country = createdClient.Country,
            PostalCode = createdClient.PostalCode,
            CompanyName = createdClient.CompanyName,
            Title = createdClient.Title,
            Comments = createdClient.Comments,
            Status = createdClient.Status,
            DateCreated = createdClient.DateCreated
        };

        _logger.LogInformation("Client created: {ClientNo}", createdClient.ClientNo);
        return CreatedAtAction(nameof(GetByClientNo), new { clientNo = createdClient.ClientNo }, ApiResponse.Ok(response, "Client created successfully"));
    }

    /// <summary>
    /// Update an existing client
    /// </summary>
    [HttpPut("{clientNo}")]
    [ProducesResponseType(typeof(ApiResponse<ClientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<ClientResponse>>> Update(string clientNo, [FromBody] ClientUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<ClientResponse>.Fail("Invalid client data", GetValidationErrors()));

        var client = new Client
        {
            ClientName = request.ClientName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            PhysicalAddress = request.PhysicalAddress,
            City = request.City,
            Country = request.Country,
            PostalCode = request.PostalCode,
            CompanyName = request.CompanyName,
            Title = request.Title,
            Comments = request.Comments,
            Status = request.Status
        };

        var updatedClient = await _clientService.UpdateAsync(clientNo, client);

        if (updatedClient == null)
            return NotFound(ApiResponse<ClientResponse>.Fail($"Client with number {clientNo} not found"));

        var response = new ClientResponse
        {
            ClientNo = updatedClient.ClientNo,
            ClientName = updatedClient.ClientName,
            Email = updatedClient.Email,
            PhoneNumber = updatedClient.PhoneNumber,
            PhysicalAddress = updatedClient.PhysicalAddress,
            City = updatedClient.City,
            Country = updatedClient.Country,
            PostalCode = updatedClient.PostalCode,
            CompanyName = updatedClient.CompanyName,
            Title = updatedClient.Title,
            Comments = updatedClient.Comments,
            Status = updatedClient.Status,
            DateCreated = updatedClient.DateCreated
        };

        _logger.LogInformation("Client updated: {ClientNo}", clientNo);
        return Ok(ApiResponse.Ok(response, "Client updated successfully"));
    }

    /// <summary>
    /// Delete a client
    /// </summary>
    [HttpDelete("{clientNo}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> Delete(string clientNo)
    {
        try
        {
            var result = await _clientService.DeleteAsync(clientNo);

            if (!result)
                return NotFound(ApiResponse.Fail($"Client with number {clientNo} not found"));

            _logger.LogInformation("Client deleted: {ClientNo}", clientNo);
            return Ok(ApiResponse.Ok("Client deleted successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse.Fail(ex.Message));
        }
    }

    /// <summary>
    /// Search clients
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponse<List<ClientResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<ClientResponse>>>> Search(
        [FromQuery] string searchTerm = "", 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10)
    {
        var clients = await _clientService.SearchAsync(searchTerm, page, pageSize);

        var response = clients.Select(c => new ClientResponse
        {
            ClientNo = c.ClientNo,
            ClientName = c.ClientName,
            Email = c.Email,
            PhoneNumber = c.PhoneNumber,
            PhysicalAddress = c.PhysicalAddress,
            City = c.City,
            Country = c.Country,
            PostalCode = c.PostalCode,
            CompanyName = c.CompanyName,
            Title = c.Title,
            Comments = c.Comments,
            Status = c.Status
        }).ToList();

        return Ok(ApiResponse.Ok(response));
    }

    private List<string> GetValidationErrors()
    {
        return ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();
    }
}
