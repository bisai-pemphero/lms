using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Security.Claims;

namespace LandManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerPortalController : ControllerBase
    {
        private readonly string _connectionString;

        public CustomerPortalController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        // GET: api/CustomerPortal/dashboard
        [HttpGet("dashboard")]
        public IActionResult GetDashboard()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value ?? User.FindFirst("username")?.Value;
            
            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Get client details
                    var clientSql = @"SELECT c.ClientNo, c.Fullname, c.PhoneNumber, c.Email, c.Address
                                     FROM Clients c
                                     INNER JOIN Users u ON c.Username = u.Username
                                     WHERE u.Username = @username";

                    object? clientData = null;
                    using (var cmd = new SqlCommand(clientSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientData = new
                                {
                                    clientNo = reader.GetString(0),
                                    fullname = reader.GetString(1),
                                    phoneNumber = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                    address = reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
                                };
                            }
                        }
                    }

                    if (clientData == null)
                        return NotFound(new { message = "Client profile not found" });

                    // Extract ClientNo for further queries
                    string clientNo = ((dynamic)clientData).clientNo;

                    // Get total plots owned
                    var plotsCountSql = @"SELECT COUNT(*) FROM Plots WHERE OfferedTo = @clientNo";
                    int totalPlots;
                    using (var cmd = new SqlCommand(plotsCountSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@clientNo", clientNo);
                        totalPlots = (int)cmd.ExecuteScalar();
                    }

                    // Get total paid amount
                    var totalPaidSql = @"SELECT ISNULL(SUM(Amount), 0) FROM PlotPayments WHERE ClientNo = @clientNo";
                    decimal totalPaid;
                    using (var cmd = new SqlCommand(totalPaidSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@clientNo", clientNo);
                        totalPaid = (decimal)cmd.ExecuteScalar();
                    }

                    // Get outstanding balance
                    var balanceSql = @"SELECT ISNULL(SUM(Balance), 0) FROM Plots WHERE OfferedTo = @clientNo";
                    decimal outstandingBalance;
                    using (var cmd = new SqlCommand(balanceSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@clientNo", clientNo);
                        outstandingBalance = (decimal)cmd.ExecuteScalar();
                    }

                    // Get recent payments (last 5)
                    var recentPayments = new List<object>();
                    var paymentsSql = @"SELECT TOP 5 PaymentDate, Amount, PaymentMethod, ReferenceNo 
                                       FROM PlotPayments 
                                       WHERE ClientNo = @clientNo 
                                       ORDER BY PaymentDate DESC";

                    using (var cmd = new SqlCommand(paymentsSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@clientNo", clientNo);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                recentPayments.Add(new
                                {
                                    paymentDate = reader.GetDateTime(0),
                                    amount = reader.GetDecimal(1),
                                    paymentMethod = reader.GetString(2),
                                    referenceNo = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                                });
                            }
                        }
                    }

                    return Ok(new
                    {
                        client = clientData,
                        totalPlots,
                        totalPaid,
                        outstandingBalance,
                        recentPayments
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving dashboard data: {ex.Message}");
            }
        }

        // GET: api/CustomerPortal/plots
        [HttpGet("plots")]
        public IActionResult GetMyPlots()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value ?? User.FindFirst("username")?.Value;
            
            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Get client number
                    var clientNoSql = @"SELECT ClientNo FROM Clients WHERE Username = @username";
                    string? clientNo = null;
                    
                    using (var cmd = new SqlCommand(clientNoSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientNo = reader.GetString(0);
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(clientNo))
                        return NotFound(new { message = "Client not found" });

                    // Get all plots for this client
                    var plots = new List<object>();
                    var plotsSql = @"SELECT p.PlotNo, p.SiteNo, s.PhysicalLocation, p.PlotSize, 
                                           p.Price, p.Balance, p.PlotStatus, p.OfferDate
                                    FROM Plots p
                                    INNER JOIN Sites s ON p.SiteNo = s.SiteCode
                                    WHERE p.OfferedTo = @clientNo
                                    ORDER BY s.PhysicalLocation, p.PlotNo";

                    using (var cmd = new SqlCommand(plotsSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@clientNo", clientNo);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                plots.Add(new
                                {
                                    plotNo = reader.GetString(0),
                                    siteNo = reader.GetString(1),
                                    physicalLocation = reader.GetString(2),
                                    plotSize = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                    price = reader.GetDecimal(4),
                                    balance = reader.GetDecimal(5),
                                    plotStatus = reader.GetString(6),
                                    offerDate = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7)
                                });
                            }
                        }
                    }

                    return Ok(new { plots });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving plots: {ex.Message}");
            }
        }

        // GET: api/CustomerPortal/payments
        [HttpGet("payments")]
        public IActionResult GetMyPayments()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value ?? User.FindFirst("username")?.Value;
            
            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Get client number
                    var clientNoSql = @"SELECT ClientNo FROM Clients WHERE Username = @username";
                    string? clientNo = null;
                    
                    using (var cmd = new SqlCommand(clientNoSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientNo = reader.GetString(0);
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(clientNo))
                        return NotFound(new { message = "Client not found" });

                    // Get all payments for this client
                    var payments = new List<object>();
                    var paymentsSql = @"SELECT pp.PaymentDate, pp.Amount, pp.PaymentMethod, pp.ReferenceNo,
                                              p.PlotNo, s.PhysicalLocation
                                       FROM PlotPayments pp
                                       INNER JOIN Plots p ON pp.PlotNo = p.PlotNo AND pp.SiteNo = p.SiteNo
                                       INNER JOIN Sites s ON p.SiteNo = s.SiteCode
                                       WHERE pp.ClientNo = @clientNo
                                       ORDER BY pp.PaymentDate DESC";

                    using (var cmd = new SqlCommand(paymentsSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@clientNo", clientNo);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                payments.Add(new
                                {
                                    paymentDate = reader.GetDateTime(0),
                                    amount = reader.GetDecimal(1),
                                    paymentMethod = reader.GetString(2),
                                    referenceNo = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                    plotNo = reader.GetString(4),
                                    physicalLocation = reader.GetString(5)
                                });
                            }
                        }
                    }

                    return Ok(new { payments });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving payments: {ex.Message}");
            }
        }

        // GET: api/CustomerPortal/documents
        [HttpGet("documents")]
        public IActionResult GetMyDocuments()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value ?? User.FindFirst("username")?.Value;
            
            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Get client number
                    var clientNoSql = @"SELECT ClientNo FROM Clients WHERE Username = @username";
                    string? clientNo = null;
                    
                    using (var cmd = new SqlCommand(clientNoSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientNo = reader.GetString(0);
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(clientNo))
                        return NotFound(new { message = "Client not found" });

                    // Get sale agreements for this client
                    var documents = new List<object>();
                    var documentsSql = @"SELECT sa.AgreementDate, sa.PurchasePrice, sa.InstallmentPeriod,
                                               p.PlotNo, s.PhysicalLocation, 'Sale Agreement' AS DocumentType
                                       FROM SaleAgreements sa
                                       INNER JOIN Plots p ON sa.PlotNo = p.PlotNo AND sa.SiteNo = p.SiteNo
                                       INNER JOIN Sites s ON p.SiteNo = s.SiteCode
                                       WHERE sa.ClientNo = @clientNo
                                       
                                       UNION ALL
                                       
                                       SELECT ol.OfferDate, ol.PurchasePrice, 0 AS InstallmentPeriod,
                                              p.PlotNo, s.PhysicalLocation, 'Offer Letter' AS DocumentType
                                       FROM OfferLetters ol
                                       INNER JOIN Plots p ON ol.PlotNo = p.PlotNo AND ol.SiteNo = p.SiteNo
                                       INNER JOIN Sites s ON p.SiteNo = s.SiteCode
                                       WHERE ol.ClientNo = @clientNo
                                       
                                       ORDER BY DocumentType, PlotNo";

                    using (var cmd = new SqlCommand(documentsSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@clientNo", clientNo);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                documents.Add(new
                                {
                                    documentDate = reader.GetDateTime(0),
                                    purchasePrice = reader.GetDecimal(1),
                                    installmentPeriod = reader.GetInt32(2),
                                    plotNo = reader.GetString(3),
                                    physicalLocation = reader.GetString(4),
                                    documentType = reader.GetString(5)
                                });
                            }
                        }
                    }

                    return Ok(new { documents });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving documents: {ex.Message}");
            }
        }

        // GET: api/CustomerPortal/next-of-kin
        [HttpGet("next-of-kin")]
        public IActionResult GetMyNextOfKin()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value ?? User.FindFirst("username")?.Value;
            
            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Get client number
                    var clientNoSql = @"SELECT ClientNo FROM Clients WHERE Username = @username";
                    string? clientNo = null;
                    
                    using (var cmd = new SqlCommand(clientNoSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientNo = reader.GetString(0);
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(clientNo))
                        return NotFound(new { message = "Client not found" });

                    // Get next of kin for this client
                    var nextOfKins = new List<object>();
                    var nextOfKinSql = @"SELECT Name, Relationship, Contact 
                                        FROM Next_of_Kins 
                                        WHERE ClientNo = @clientNo
                                        ORDER BY Name";

                    using (var cmd = new SqlCommand(nextOfKinSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@clientNo", clientNo);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                nextOfKins.Add(new
                                {
                                    name = reader.GetString(0),
                                    relationship = reader.GetString(1),
                                    contact = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
                                });
                            }
                        }
                    }

                    return Ok(new { nextOfKins });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving next of kin: {ex.Message}");
            }
        }
    }
}
