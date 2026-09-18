namespace LandManagement.Api.Shared.Models;

/// <summary>
/// Pagination parameters for search and filter operations
/// </summary>
public class PaginationParams
{
    private const int MaxPageSize = 100;
    
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; }

    public int EffectivePageSize => PageSize > MaxPageSize ? MaxPageSize : PageSize;
}
