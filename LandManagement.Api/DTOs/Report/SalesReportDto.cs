namespace LandManagement.Api.DTOs.Report;

/// <summary>
/// Request model for sales report with date range and location filter
/// </summary>
public class SalesReportRequest
{
    /// <summary>
    /// District/Location to filter by
    /// </summary>
    public string? Location { get; set; }
    
    /// <summary>
    /// Start date for the report period
    /// </summary>
    public DateTime StartDate { get; set; }
    
    /// <summary>
    /// End date for the report period
    /// </summary>
    public DateTime EndDate { get; set; }
}

/// <summary>
/// Response model for a single sales record
/// </summary>
public class SalesRecordDto
{
    public string Fullname { get; set; } = string.Empty;
    public string PhysicalLocation { get; set; } = string.Empty;
    public string PlotNo { get; set; } = string.Empty;
    public decimal AmountPaid { get; set; }
    public decimal NewBalance { get; set; }
    public string PaymentMode { get; set; } = string.Empty;
    public DateTime DatePaid { get; set; }
}

/// <summary>
/// Response model for sales statistics summary
/// </summary>
public class SalesStatsDto
{
    public decimal ExpectedIncome { get; set; }
    public decimal CollectedAmount { get; set; }
    public decimal Balance { get; set; }
    public decimal OverdueAmount { get; set; }
}

/// <summary>
/// Combined response for sales report
/// </summary>
public class SalesReportResponse
{
    public SalesStatsDto Stats { get; set; } = new();
    public List<SalesRecordDto> Records { get; set; } = new();
    public List<OverduePlotDto> OverduePlots { get; set; } = new();
}

/// <summary>
/// Model for overdue plot information
/// </summary>
public class OverduePlotDto
{
    public string PhysicalLocation { get; set; } = string.Empty;
    public string PlotNo { get; set; } = string.Empty;
    public string Fullname { get; set; } = string.Empty;
    public DateTime DateOffered { get; set; }
    public int OfferPeriod { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal Balance { get; set; }
    public DateTime? DueDate { get; set; }
    public int? MonthsOverdue { get; set; }
}
