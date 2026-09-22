using LandManagement.Api.DTOs.Document;
using LandManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandManagement.Api.Controllers;

/// <summary>
/// Controller for document generation operations based on legacy OfferLetter.aspx.cs, SaleAgreementDocument.aspx.cs, and ChangeofOwnershipDocument.aspx.cs
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DocumentGenerationController : ControllerBase
{
    private readonly IDocumentService _service;
    private readonly ILogger<DocumentGenerationController> _logger;

    public DocumentGenerationController(IDocumentService service, ILogger<DocumentGenerationController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Generate offer letter data for a specific plot
    /// Based on OfferLetter.aspx.cs LoadClientDetails() and LoadPlotDetails()
    /// </summary>
    /// <param name="plotNo">Plot number</param>
    /// <returns>Offer letter data including client info, plot details, and next of kins</returns>
    [HttpGet("offer-letter/{plotNo}")]
    [ProducesResponseType(typeof(OfferLetterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OfferLetterDto>> GetOfferLetter(string plotNo)
    {
        try
        {
            var user = User.Identity?.Name ?? "Unknown";
            _logger.LogInformation("User {User} requesting offer letter for plot: {PlotNo}", user, plotNo);

            var offerLetter = await _service.GetOfferLetterDataAsync(plotNo);
            
            return Ok(offerLetter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating offer letter for plot: {PlotNo}", plotNo);
            return StatusCode(500, new { Success = false, Message = "An error occurred while generating offer letter" });
        }
    }

    /// <summary>
    /// Generate sale agreement document data
    /// Based on SaleAgreementDocument.aspx.cs logic
    /// </summary>
    /// <param name="request">Request with district, site code, or sale agreement ID</param>
    /// <returns>Sale agreement document data</returns>
    [HttpPost("sale-agreement")]
    [ProducesResponseType(typeof(SaleAgreementDocumentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SaleAgreementDocumentDto>> GetSaleAgreementDocument([FromBody] SaleAgreementDocumentRequest request)
    {
        try
        {
            var user = User.Identity?.Name ?? "Unknown";
            _logger.LogInformation("User {User} requesting sale agreement document", user);

            var document = await _service.GetSaleAgreementDocumentDataAsync(request);
            
            if (document == null)
                return NotFound(new { Success = false, Message = "Sale agreement not found" });

            return Ok(document);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating sale agreement document");
            return StatusCode(500, new { Success = false, Message = "An error occurred while generating sale agreement document" });
        }
    }

    /// <summary>
    /// Generate change of ownership document data
    /// Based on ChangeofOwnershipDocument.aspx.cs logic
    /// </summary>
    /// <param name="receiptNo">Receipt number for the ownership change</param>
    /// <returns>Change of ownership document data</returns>
    [HttpGet("change-of-ownership/{receiptNo}")]
    [ProducesResponseType(typeof(ChangeOfOwnershipDocumentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ChangeOfOwnershipDocumentDto>> GetChangeOfOwnershipDocument(string receiptNo)
    {
        try
        {
            var user = User.Identity?.Name ?? "Unknown";
            _logger.LogInformation("User {User} requesting change of ownership document for receipt: {ReceiptNo}", user, receiptNo);

            var document = await _service.GetChangeOfOwnershipDocumentDataAsync(receiptNo);
            
            if (document == null)
                return NotFound(new { Success = false, Message = "Change of ownership document not found" });

            return Ok(document);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating change of ownership document");
            return StatusCode(500, new { Success = false, Message = "An error occurred while generating change of ownership document" });
        }
    }

    /// <summary>
    /// Convert a number to words (utility function used in document generation)
    /// Based on OfferLetter.aspx.cs ConvertNumberToWords()
    /// </summary>
    /// <param name="number">Number to convert</param>
    /// <returns>Number in words</returns>
    [HttpGet("convert-number-to-words/{number}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public ActionResult<string> ConvertNumberToWords(long number)
    {
        try
        {
            var words = _service.ConvertNumberToWords(number);
            return Ok(words);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error converting number to words");
            return StatusCode(500, new { Success = false, Message = "An error occurred while converting number to words" });
        }
    }

    /// <summary>
    /// Get all documents available for a specific plot (offer letter, sale agreement, etc.)
    /// </summary>
    /// <param name="plotNo">Plot number</param>
    /// <returns>List of available document types for the plot</returns>
    [HttpGet("plot/{plotNo}/available")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> GetAvailableDocuments(string plotNo)
    {
        try
        {
            var user = User.Identity?.Name ?? "Unknown";
            _logger.LogInformation("User {User} checking available documents for plot: {PlotNo}", user, plotNo);

            var availableDocs = new List<string>();

            // Check if offer letter is available
            try
            {
                await _service.GetOfferLetterDataAsync(plotNo);
                availableDocs.Add("OfferLetter");
            }
            catch
            {
                // Offer letter not available
            }

            // Additional document checks can be added here

            return Ok(new 
            { 
                PlotNo = plotNo,
                AvailableDocuments = availableDocs,
                TotalCount = availableDocs.Count
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking available documents");
            return StatusCode(500, new { Success = false, Message = "An error occurred while checking available documents" });
        }
    }
}
