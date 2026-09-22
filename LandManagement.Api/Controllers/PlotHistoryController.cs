using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace LandManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PlotHistoryController : ControllerBase
    {
        private readonly string _connectionString;

        public PlotHistoryController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        // GET: api/PlotHistory/site/{siteNo}
        [HttpGet("site/{siteNo}")]
        public IActionResult GetPlotHistoryBySite(string siteNo)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var sql = @"
                        WITH PlotHistory AS (
                            -- Plot Withdrawals
                            SELECT
                                pw.PlotNo,
                                pw.SiteNo,
                                s.PhysicalLocation,
                                pw.OfferedTo AS Owner,
                                c.Fullname AS OwnerName,
                                'Withdrawal' AS ActionType,
                                pw.WithdrawReason AS Reason,
                                pw.WithdrawDate AS ActionDate,
                                pw.DateAllocated AS AllocationDate,
                                NULL AS PreviousOwner,
                                NULL AS PreviousOwnerName,
                                NULL AS CurrentOwnerFromPlots,
                                NULL AS CurrentOwnerNameFromPlots
                            FROM [dbo].[PlotWithdrawal] pw
                            LEFT JOIN [dbo].[Sites] s ON pw.SiteNo = s.SiteCode
                            LEFT JOIN [dbo].[Clients] c ON pw.OfferedTo = c.ClientNo

                            UNION ALL

                            -- Change of Ownership (Current Owner)
                            SELECT
                                co.Plot_Number AS PlotNo,
                                co.Site_Number AS SiteNo,
                                s.PhysicalLocation,
                                co.Current_Owner AS Owner,
                                c_current.Fullname AS OwnerName,
                                'Ownership Change' AS ActionType,
                                'Transfer' AS Reason,
                                co.Date AS ActionDate,
                                NULL AS AllocationDate,
                                co.Previous_Owner AS PreviousOwner,
                                c_prev.Fullname AS PreviousOwnerName,
                                NULL AS CurrentOwnerFromPlots,
                                NULL AS CurrentOwnerNameFromPlots
                            FROM [dbo].[ChangeofOwnership] co
                            LEFT JOIN [dbo].[Sites] s ON co.Site_Number = s.SiteCode
                            LEFT JOIN [dbo].[Clients] c_current ON co.Current_Owner = c_current.ClientNo
                            LEFT JOIN [dbo].[Clients] c_prev ON co.Previous_Owner = c_prev.ClientNo

                            UNION ALL

                            -- Change of Ownership (Previous Owner - to show the ownership period)
                            SELECT
                                co.Plot_Number AS PlotNo,
                                co.Site_Number AS SiteNo,
                                s.PhysicalLocation,
                                co.Previous_Owner AS Owner,
                                c_prev.Fullname AS OwnerName,
                                'Ownership Ended' AS ActionType,
                                'Changed Ownership' AS Reason,
                                co.Date AS ActionDate,
                                NULL AS AllocationDate,
                                NULL AS PreviousOwner,
                                NULL AS PreviousOwnerName,
                                NULL AS CurrentOwnerFromPlots,
                                NULL AS CurrentOwnerNameFromPlots
                            FROM [dbo].[ChangeofOwnership] co
                            LEFT JOIN [dbo].[Sites] s ON co.Site_Number = s.SiteCode
                            LEFT JOIN [dbo].[Clients] c_prev ON co.Previous_Owner = c_prev.ClientNo
                        )

                        SELECT
                            ph.PhysicalLocation, 
                            ph.PlotNo, 
                            ph.OwnerName, 
                            ph.ActionType, 
                            ph.Reason,
                            ph.ActionDate, 
                            ph.AllocationDate, 
                            ph.PreviousOwnerName, 
                            c_current.Fullname AS CurrentOwnerName,
                            p.PlotStatus
                        FROM PlotHistory ph
                        LEFT JOIN [dbo].[Plots] p ON ph.PlotNo = p.PlotNo AND ph.SiteNo = p.SiteNo
                        LEFT JOIN [dbo].[Clients] c_current ON p.OfferedTo = c_current.ClientNo
                        WHERE ph.SiteNo = @siteNo
                        ORDER BY ph.PlotNo, ph.SiteNo, ph.ActionDate DESC";

                    var plotHistory = new List<object>();

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@siteNo", siteNo);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                plotHistory.Add(new
                                {
                                    physicalLocation = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                                    plotNo = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                    ownerName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    actionType = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                    reason = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                    actionDate = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                                    allocationDate = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                                    previousOwnerName = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                                    currentOwnerName = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                                    plotStatus = reader.IsDBNull(9) ? string.Empty : reader.GetString(9)
                                });
                            }
                        }
                    }

                    return Ok(plotHistory);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving plot history: {ex.Message}");
            }
        }

        // GET: api/PlotHistory/district/{district}
        [HttpGet("district/{district}")]
        public IActionResult GetPlotHistoryByDistrict(string district)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // First get all sites in the district
                    var sitesSql = @"SELECT SiteCode FROM Sites WHERE District = @district";
                    var siteCodes = new List<string>();

                    using (var cmd = new SqlCommand(sitesSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@district", district);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                siteCodes.Add(reader.GetString(0));
                            }
                        }
                    }

                    if (siteCodes.Count == 0)
                        return NotFound(new { message = $"No sites found in district {district}" });

                    // Get plot history for all sites in the district
                    var sql = @"
                        WITH PlotHistory AS (
                            -- Plot Withdrawals
                            SELECT
                                pw.PlotNo,
                                pw.SiteNo,
                                s.PhysicalLocation,
                                s.District,
                                pw.OfferedTo AS Owner,
                                c.Fullname AS OwnerName,
                                'Withdrawal' AS ActionType,
                                pw.WithdrawReason AS Reason,
                                pw.WithdrawDate AS ActionDate,
                                pw.DateAllocated AS AllocationDate,
                                NULL AS PreviousOwner,
                                NULL AS PreviousOwnerName,
                                NULL AS CurrentOwnerFromPlots,
                                NULL AS CurrentOwnerNameFromPlots
                            FROM [dbo].[PlotWithdrawal] pw
                            LEFT JOIN [dbo].[Sites] s ON pw.SiteNo = s.SiteCode
                            LEFT JOIN [dbo].[Clients] c ON pw.OfferedTo = c.ClientNo

                            UNION ALL

                            -- Change of Ownership (Current Owner)
                            SELECT
                                co.Plot_Number AS PlotNo,
                                co.Site_Number AS SiteNo,
                                s.PhysicalLocation,
                                s.District,
                                co.Current_Owner AS Owner,
                                c_current.Fullname AS OwnerName,
                                'Ownership Change' AS ActionType,
                                'Transfer' AS Reason,
                                co.Date AS ActionDate,
                                NULL AS AllocationDate,
                                co.Previous_Owner AS PreviousOwner,
                                c_prev.Fullname AS PreviousOwnerName,
                                NULL AS CurrentOwnerFromPlots,
                                NULL AS CurrentOwnerNameFromPlots
                            FROM [dbo].[ChangeofOwnership] co
                            LEFT JOIN [dbo].[Sites] s ON co.Site_Number = s.SiteCode
                            LEFT JOIN [dbo].[Clients] c_current ON co.Current_Owner = c_current.ClientNo
                            LEFT JOIN [dbo].[Clients] c_prev ON co.Previous_Owner = c_prev.ClientNo

                            UNION ALL

                            -- Change of Ownership (Previous Owner)
                            SELECT
                                co.Plot_Number AS PlotNo,
                                co.Site_Number AS SiteNo,
                                s.PhysicalLocation,
                                s.District,
                                co.Previous_Owner AS Owner,
                                c_prev.Fullname AS OwnerName,
                                'Ownership Ended' AS ActionType,
                                'Changed Ownership' AS Reason,
                                co.Date AS ActionDate,
                                NULL AS AllocationDate,
                                NULL AS PreviousOwner,
                                NULL AS PreviousOwnerName,
                                NULL AS CurrentOwnerFromPlots,
                                NULL AS CurrentOwnerNameFromPlots
                            FROM [dbo].[ChangeofOwnership] co
                            LEFT JOIN [dbo].[Sites] s ON co.Site_Number = s.SiteCode
                            LEFT JOIN [dbo].[Clients] c_prev ON co.Previous_Owner = c_prev.ClientNo
                        )

                        SELECT
                            ph.District,
                            ph.PhysicalLocation, 
                            ph.PlotNo, 
                            ph.OwnerName, 
                            ph.ActionType, 
                            ph.Reason,
                            ph.ActionDate, 
                            ph.AllocationDate, 
                            ph.PreviousOwnerName, 
                            c_current.Fullname AS CurrentOwnerName,
                            p.PlotStatus
                        FROM PlotHistory ph
                        LEFT JOIN [dbo].[Plots] p ON ph.PlotNo = p.PlotNo AND ph.SiteNo = p.SiteNo
                        LEFT JOIN [dbo].[Clients] c_current ON p.OfferedTo = c_current.ClientNo
                        WHERE ph.District = @district
                        ORDER BY ph.PhysicalLocation, ph.PlotNo, ph.ActionDate DESC";

                    var plotHistory = new List<object>();

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@district", district);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                plotHistory.Add(new
                                {
                                    district = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                                    physicalLocation = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                    plotNo = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    ownerName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                    actionType = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                    reason = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                    actionDate = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                                    allocationDate = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                                    previousOwnerName = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                                    currentOwnerName = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                                    plotStatus = reader.IsDBNull(10) ? string.Empty : reader.GetString(10)
                                });
                            }
                        }
                    }

                    return Ok(plotHistory);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving plot history: {ex.Message}");
            }
        }

        // GET: api/PlotHistory/plot/{siteNo}/{plotNo}
        [HttpGet("plot/{siteNo}/{plotNo}")]
        public IActionResult GetPlotHistoryByPlot(string siteNo, string plotNo)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var sql = @"
                        WITH PlotHistory AS (
                            -- Plot Withdrawals
                            SELECT
                                pw.PlotNo,
                                pw.SiteNo,
                                s.PhysicalLocation,
                                pw.OfferedTo AS Owner,
                                c.Fullname AS OwnerName,
                                'Withdrawal' AS ActionType,
                                pw.WithdrawReason AS Reason,
                                pw.WithdrawDate AS ActionDate,
                                pw.DateAllocated AS AllocationDate,
                                NULL AS PreviousOwner,
                                NULL AS PreviousOwnerName
                            FROM [dbo].[PlotWithdrawal] pw
                            LEFT JOIN [dbo].[Sites] s ON pw.SiteNo = s.SiteCode
                            LEFT JOIN [dbo].[Clients] c ON pw.OfferedTo = c.ClientNo
                            WHERE pw.PlotNo = @plotNo AND pw.SiteNo = @siteNo

                            UNION ALL

                            -- Change of Ownership
                            SELECT
                                co.Plot_Number AS PlotNo,
                                co.Site_Number AS SiteNo,
                                s.PhysicalLocation,
                                co.Current_Owner AS Owner,
                                c_current.Fullname AS OwnerName,
                                'Ownership Change' AS ActionType,
                                'Transfer' AS Reason,
                                co.Date AS ActionDate,
                                NULL AS AllocationDate,
                                co.Previous_Owner AS PreviousOwner,
                                c_prev.Fullname AS PreviousOwnerName
                            FROM [dbo].[ChangeofOwnership] co
                            LEFT JOIN [dbo].[Sites] s ON co.Site_Number = s.SiteCode
                            LEFT JOIN [dbo].[Clients] c_current ON co.Current_Owner = c_current.ClientNo
                            LEFT JOIN [dbo].[Clients] c_prev ON co.Previous_Owner = c_prev.ClientNo
                            WHERE co.Plot_Number = @plotNo AND co.Site_Number = @siteNo
                        )

                        SELECT
                            ph.PhysicalLocation, 
                            ph.PlotNo, 
                            ph.OwnerName, 
                            ph.ActionType, 
                            ph.Reason,
                            ph.ActionDate, 
                            ph.AllocationDate, 
                            ph.PreviousOwnerName,
                            p.PlotStatus
                        FROM PlotHistory ph
                        LEFT JOIN [dbo].[Plots] p ON ph.PlotNo = p.PlotNo AND ph.SiteNo = p.SiteNo
                        WHERE ph.PlotNo = @plotNo AND ph.SiteNo = @siteNo
                        ORDER BY ph.ActionDate DESC";

                    var plotHistory = new List<object>();

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@plotNo", plotNo);
                        command.Parameters.AddWithValue("@siteNo", siteNo);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                plotHistory.Add(new
                                {
                                    physicalLocation = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                                    plotNo = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                    ownerName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    actionType = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                    reason = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                    actionDate = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                                    allocationDate = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                                    previousOwnerName = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                                    plotStatus = reader.IsDBNull(8) ? string.Empty : reader.GetString(8)
                                });
                            }
                        }
                    }

                    if (plotHistory.Count == 0)
                        return NotFound(new { message = $"No history found for plot {plotNo} at site {siteNo}" });

                    return Ok(plotHistory);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving plot history: {ex.Message}");
            }
        }

        // GET: api/PlotHistory/summary
        [HttpGet("summary")]
        public IActionResult GetPlotHistorySummary()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var sql = @"
                        SELECT 
                            COUNT(*) AS TotalEvents,
                            SUM(CASE WHEN ActionType = 'Withdrawal' THEN 1 ELSE 0 END) AS WithdrawalCount,
                            SUM(CASE WHEN ActionType LIKE '%Ownership%' THEN 1 ELSE 0 END) AS OwnershipChangeCount
                        FROM (
                            SELECT 'Withdrawal' AS ActionType FROM [dbo].[PlotWithdrawal]
                            UNION ALL
                            SELECT 'Ownership Change' AS ActionType FROM [dbo].[ChangeofOwnership]
                            UNION ALL
                            SELECT 'Ownership Ended' AS ActionType FROM [dbo].[ChangeofOwnership]
                        ) AS AllEvents";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var summary = new
                                {
                                    totalEvents = reader.GetInt32(0),
                                    withdrawalCount = reader.GetInt32(1),
                                    ownershipChangeCount = reader.GetInt32(2)
                                };

                                return Ok(summary);
                            }
                        }
                    }

                    return Ok(new { totalEvents = 0, withdrawalCount = 0, ownershipChangeCount = 0 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving plot history summary: {ex.Message}");
            }
        }
    }
}
