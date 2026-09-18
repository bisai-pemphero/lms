namespace LandManagement.Api.DTOs.Dashboard;

public class DashboardSummaryDto
{
    public int TotalSites { get; set; }
    public int TotalPlots { get; set; }
    public int AvailablePlots { get; set; }
    public int AllocatedPlots { get; set; }
    public int FullyPaidPlots { get; set; }
    public int WithdrawnPlots { get; set; }
    public decimal TotalSales { get; set; }
    public decimal TotalCollected { get; set; }
    public decimal OutstandingBalance { get; set; }
    public int TotalClients { get; set; }
    public int TotalCreditors { get; set; }
}

public class SitePerformanceDto
{
    public string SiteCode { get; set; } = string.Empty;
    public string? SiteName { get; set; }
    public int TotalPlots { get; set; }
    public int AvailablePlots { get; set; }
    public int SoldPlots { get; set; }
    public decimal TotalValue { get; set; }
    public decimal Collected { get; set; }
}

public class RecentTransactionDto
{
    public string Type { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
}
