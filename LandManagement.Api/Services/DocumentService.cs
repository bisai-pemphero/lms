using LandManagement.Api.DTOs.Document;
using Microsoft.EntityFrameworkCore;
using LandManagement.Api.Data;

namespace LandManagement.Api.Services;

/// <summary>
/// Service interface for document generation operations
/// </summary>
public interface IDocumentService
{
    Task<OfferLetterDto> GetOfferLetterDataAsync(string plotNo);
    Task<SaleAgreementDocumentDto?> GetSaleAgreementDocumentDataAsync(SaleAgreementDocumentRequest request);
    Task<ChangeOfOwnershipDocumentDto?> GetChangeOfOwnershipDocumentDataAsync(string receiptNo);
    string ConvertNumberToWords(long number);
}

/// <summary>
/// Implementation of document service based on legacy OfferLetter.aspx.cs, SaleAgreementDocument.aspx.cs, and ChangeofOwnershipDocument.aspx.cs
/// </summary>
public class DocumentService : IDocumentService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DocumentService> _logger;

    public DocumentService(
        ApplicationDbContext context,
        ILogger<DocumentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<OfferLetterDto> GetOfferLetterDataAsync(string plotNo)
    {
        // Based on OfferLetter.aspx.cs LoadClientDetails() and LoadPlotDetails()
        var clientQuery = from c in _context.Clients
                          join pa in _context.PlotAllocations on c.ClientNo equals pa.ClientNo
                          where pa.PlotNo == plotNo
                          select new
                          {
                              c.ClientNo,
                              c.Fullname,
                              c.Address,
                              c.PhoneNo,
                              c.PhoneNo2,
                              c.Email
                          };

        var clientData = await clientQuery.FirstOrDefaultAsync();
        
        if (clientData == null)
            throw new Exception($"Client not found for plot: {plotNo}");

        var plotQuery = from p in _context.Plots
                        join s in _context.Sites on p.SiteNo equals s.SiteCode
                        where p.PlotNo == plotNo
                        select new
                        {
                            p.PlotNo,
                            p.PlotSize,
                            s.PhysicalLocation,
                            p.AgreedPrice,
                            p.DateOffered,
                            p.PriceCategory,
                            p.OfferPeriod
                        };

        var plotData = await plotQuery.FirstOrDefaultAsync();
        
        if (plotData == null)
            throw new Exception($"Plot not found: {plotNo}");

        // Get next of kins - based on OfferLetter.aspx.cs LoadNextofKinDetails()
        var nextOfKinsQuery = from n in _context.NextOfKins
                              join p in _context.Plots on n.ClientNo equals p.OfferedTo
                              where p.PlotNo == plotNo
                              select new NextOfKinDto
                              {
                                  Name = n.Name ?? string.Empty,
                                  Relationship = n.Relationship ?? string.Empty,
                                  Contact = n.Contact ?? string.Empty
                              };

        var nextOfKins = await nextOfKinsQuery.ToListAsync();

        // Convert price to words - based on OfferLetter.aspx.cs ConvertNumberToWords()
        long priceValue = (long)(plotData.AgreedPrice ?? 0);
        string priceInWords = ConvertNumberToWords(priceValue);

        var offerLetter = new OfferLetterDto
        {
            ClientNo = clientData.ClientNo ?? string.Empty,
            ClientName = clientData.Fullname ?? string.Empty,
            Address = clientData.Address ?? string.Empty,
            Phone = !string.IsNullOrEmpty(clientData.PhoneNo2) 
                ? $"{clientData.PhoneNo} / {clientData.PhoneNo2}" 
                : clientData.PhoneNo ?? string.Empty,
            Email = clientData.Email ?? string.Empty,
            PlotNo = plotData.PlotNo ?? string.Empty,
            PlotSize = $"{plotData.PlotSize} Square Meters",
            PhysicalLocation = plotData.PhysicalLocation ?? string.Empty,
            AgreedPrice = plotData.AgreedPrice ?? 0,
            DateOffered = plotData.DateOffered,
            PriceCategory = plotData.PriceCategory ?? string.Empty,
            OfferPeriod = !string.IsNullOrEmpty(plotData.OfferPeriod) ? int.Parse(plotData.OfferPeriod) : 0,
            PriceInWords = priceInWords,
            NextOfKins = nextOfKins
        };

        _logger.LogInformation("Generated offer letter data for plot: {PlotNo}", plotNo);
        
        return offerLetter;
    }

    public async Task<SaleAgreementDocumentDto?> GetSaleAgreementDocumentDataAsync(SaleAgreementDocumentRequest request)
    {
        // Based on SaleAgreementDocument.aspx.cs logic
        SaleAgreement? agreement;
        
        if (request.SaleAgreementId.HasValue)
        {
            agreement = await _context.SaleAgreements.FindAsync(request.SaleAgreementId.Value);
        }
        else if (!string.IsNullOrEmpty(request.District) && !string.IsNullOrEmpty(request.SiteCode))
        {
            agreement = await _context.SaleAgreements
                .FirstOrDefaultAsync(sa => sa.District == request.District && sa.SiteCode == request.SiteCode);
        }
        else
        {
            return null;
        }

        if (agreement == null)
            return null;

        // Get client and plot information
        var query = from sa in _context.SaleAgreements
                    join c in _context.Clients on sa.ClientNo equals c.ClientNo
                    join s in _context.Sites on sa.SiteCode equals s.SiteCode
                    where sa.Id == agreement.Id
                    select new SaleAgreementDocumentDto
                    {
                        Id = sa.Id,
                        ClientName = c.Fullname ?? string.Empty,
                        PhysicalLocation = s.PhysicalLocation ?? string.Empty,
                        PlotNumbers = sa.PlotNumbers ?? string.Empty,
                        OfferDate = sa.DateCreated
                    };

        var result = await query.FirstOrDefaultAsync();
        
        if (result != null)
        {
            _logger.LogInformation("Generated sale agreement document data for ID: {Id}", agreement.Id);
        }

        return result;
    }

    public async Task<ChangeOfOwnershipDocumentDto?> GetChangeOfOwnershipDocumentDataAsync(string receiptNo)
    {
        // Based on ChangeofOwnershipDocument.aspx.cs logic
        // First find the ownership change record by receipt number
        var ownershipChange = await _context.ChangeOfOwnerships
            .FirstOrDefaultAsync(co => co.ReceiptNo == receiptNo);

        if (ownershipChange == null)
            return null;

        // Get related sale agreement and client information
        var query = from co in _context.ChangeOfOwnerships
                    join sa in _context.SaleAgreements on co.SaleAgreementId equals sa.Id
                    join prevClient in _context.Clients on sa.ClientNo equals prevClient.ClientNo
                    join newClient in _context.Clients on co.NewClientId equals newClient.ClientNo
                    join s in _context.Sites on sa.SiteCode equals s.SiteCode
                    where co.ReceiptNo == receiptNo
                    select new ChangeOfOwnershipDocumentDto
                    {
                        SaleAgreementId = co.SaleAgreementId.ToString(),
                        PreviousOwner = prevClient.Fullname ?? string.Empty,
                        NewOwner = newClient.Fullname ?? string.Empty,
                        SiteName = s.SiteName ?? string.Empty,
                        PlotNumbers = sa.PlotNumbers ?? string.Empty,
                        PlotSize = sa.PlotSize ?? string.Empty,
                        Fee = co.Fee ?? 0,
                        PaymentMode = co.PaymentMode ?? string.Empty,
                        ProofOfPayment = co.ProofOfPayment ?? string.Empty,
                        PreparedBy = co.PreparedBy ?? string.Empty
                    };

        var result = await query.FirstOrDefaultAsync();
        
        if (result != null)
        {
            _logger.LogInformation("Generated change of ownership document for receipt: {ReceiptNo}", receiptNo);
        }

        return result;
    }

    public string ConvertNumberToWords(long number)
    {
        // Based on OfferLetter.aspx.cs ConvertNumberToWords() method
        if (number == 0)
            return "Zero";

        if (number < 0)
            return "Negative " + ConvertNumberToWords(Math.Abs(number));

        string words = "";

        if ((number / 1000000000) > 0)
        {
            words += ConvertNumberToWords(number / 1000000000) + " Billion ";
            number %= 1000000000;
        }

        if ((number / 1000000) > 0)
        {
            words += ConvertNumberToWords(number / 1000000) + " Million ";
            number %= 1000000;
        }

        if ((number / 1000) > 0)
        {
            words += ConvertNumberToWords(number / 1000) + " Thousand ";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            words += ConvertNumberToWords(number / 100) + " Hundred ";
            number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += "and ";

            string[] unitsMap =
            {
                "", "One", "Two", "Three", "Four",
                "Five", "Six", "Seven", "Eight", "Nine"
            };

            string[] tensMap =
            {
                "", "Ten", "Twenty", "Thirty", "Forty",
                "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"
            };

            string[] teensMap =
            {
                "Eleven", "Twelve", "Thirteen", "Fourteen",
                "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"
            };

            if (number < 10)
            {
                words += unitsMap[number];
            }
            else if (number == 10)
            {
                words += "Ten";
            }
            else if (number < 20)
            {
                words += teensMap[number - 11];
            }
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                {
                    words += "-" + unitsMap[number % 10];
                }
            }
        }

        return words.Trim();
    }
}
