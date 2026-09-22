namespace LandManagement.Api.DTOs.Report;

/// <summary>
/// Model for reminder record - customers with balance > 0 and offer period > 12 months
/// </summary>
public class ReminderRecordDto
{
    public string PlotNo { get; set; } = string.Empty;
    public string PhysicalLocation { get; set; } = string.Empty;
    public string Fullname { get; set; } = string.Empty;
    public string PhoneNo { get; set; } = string.Empty;
    public decimal MonthlyInstallment { get; set; }
    public decimal AgreedPrice { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal Balance { get; set; }
    public decimal PenaltyCharge { get; set; }
}

/// <summary>
/// Response model for reminder list
/// </summary>
public class ReminderResponse
{
    public List<ReminderRecordDto> Records { get; set; } = new();
    public int TotalCount { get; set; }
}
