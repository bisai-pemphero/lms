using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace LandManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DistrictsController : ControllerBase
    {
        private readonly string _connectionString;

        public DistrictsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        // GET: api/Districts
        [HttpGet]
        public IActionResult GetAllDistricts()
        {
            var districts = new List<object>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var sql = @"SELECT DistrictName, Region 
                               FROM Districts 
                               ORDER BY DistrictName";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                districts.Add(new
                                {
                                    districtName = reader.GetString(0),
                                    region = reader.IsDBNull(1) ? string.Empty : reader.GetString(1)
                                });
                            }
                        }
                    }
                }

                return Ok(districts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving districts: {ex.Message}");
            }
        }

        // GET: api/Districts/{name}
        [HttpGet("{name}")]
        public IActionResult GetDistrictByName(string name)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var sql = @"SELECT DistrictName, Region 
                               FROM Districts 
                               WHERE DistrictName = @districtName";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@districtName", name);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var district = new
                                {
                                    districtName = reader.GetString(0),
                                    region = reader.IsDBNull(1) ? string.Empty : reader.GetString(1)
                                };

                                return Ok(district);
                            }
                        }
                    }
                }

                return NotFound(new { message = $"District '{name}' not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving district: {ex.Message}");
            }
        }

        // POST: api/Districts
        [HttpPost]
        public IActionResult AddDistrict([FromBody] DistrictDto dto)
        {
            if (string.IsNullOrEmpty(dto.DistrictName))
                return BadRequest(new { message = "District name is required" });

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Check if district already exists
                    var checkSql = @"SELECT COUNT(*) FROM Districts WHERE DistrictName = @districtName";
                    
                    using (var checkCmd = new SqlCommand(checkSql, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@districtName", dto.DistrictName);
                        var count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                            return Conflict(new { message = "District already exists" });
                    }

                    // Insert new district
                    var insertSql = @"INSERT INTO Districts (DistrictName, Region) 
                                     VALUES (@districtName, @region)";

                    using (var insertCmd = new SqlCommand(insertSql, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@districtName", dto.DistrictName);
                        insertCmd.Parameters.AddWithValue("@region", string.IsNullOrEmpty(dto.Region) ? (object)DBNull.Value : dto.Region);

                        insertCmd.ExecuteNonQuery();
                    }
                }

                return CreatedAtAction(nameof(GetDistrictByName), new { name = dto.DistrictName }, 
                    new { message = "District added successfully", districtName = dto.DistrictName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding district: {ex.Message}");
            }
        }

        // PUT: api/Districts/{name}
        [HttpPut("{name}")]
        public IActionResult UpdateDistrict(string name, [FromBody] DistrictDto dto)
        {
            if (string.IsNullOrEmpty(dto.DistrictName))
                return BadRequest(new { message = "District name is required" });

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Check if district exists
                    var checkSql = @"SELECT COUNT(*) FROM Districts WHERE DistrictName = @districtName";
                    
                    using (var checkCmd = new SqlCommand(checkSql, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@districtName", name);
                        var count = (int)checkCmd.ExecuteScalar();

                        if (count == 0)
                            return NotFound(new { message = $"District '{name}' not found" });
                    }

                    // Update district
                    var updateSql = @"UPDATE Districts 
                                     SET DistrictName = @newDistrictName, 
                                         Region = @region 
                                     WHERE DistrictName = @oldDistrictName";

                    using (var updateCmd = new SqlCommand(updateSql, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@newDistrictName", dto.DistrictName);
                        updateCmd.Parameters.AddWithValue("@region", string.IsNullOrEmpty(dto.Region) ? (object)DBNull.Value : dto.Region);
                        updateCmd.Parameters.AddWithValue("@oldDistrictName", name);

                        var rowsAffected = updateCmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                            return NotFound(new { message = $"District '{name}' not found" });
                    }
                }

                return Ok(new { message = "District updated successfully", districtName = dto.DistrictName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating district: {ex.Message}");
            }
        }

        // DELETE: api/Districts/{name}
        [HttpDelete("{name}")]
        public IActionResult DeleteDistrict(string name)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Check if district exists
                    var checkSql = @"SELECT COUNT(*) FROM Districts WHERE DistrictName = @districtName";
                    
                    using (var checkCmd = new SqlCommand(checkSql, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@districtName", name);
                        var count = (int)checkCmd.ExecuteScalar();

                        if (count == 0)
                            return NotFound(new { message = $"District '{name}' not found" });
                    }

                    // Delete district
                    var deleteSql = @"DELETE FROM Districts WHERE DistrictName = @districtName";

                    using (var deleteCmd = new SqlCommand(deleteSql, connection))
                    {
                        deleteCmd.Parameters.AddWithValue("@districtName", name);

                        var rowsAffected = deleteCmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                            return NotFound(new { message = $"District '{name}' not found" });
                    }
                }

                return Ok(new { message = "District deleted successfully", districtName = name });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting district: {ex.Message}");
            }
        }

        // GET: api/Districts/by-region/{region}
        [HttpGet("by-region/{region}")]
        public IActionResult GetDistrictsByRegion(string region)
        {
            var districts = new List<object>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var sql = @"SELECT DistrictName, Region 
                               FROM Districts 
                               WHERE Region = @region
                               ORDER BY DistrictName";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@region", region);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                districts.Add(new
                                {
                                    districtName = reader.GetString(0),
                                    region = reader.GetString(1)
                                });
                            }
                        }
                    }
                }

                if (districts.Count == 0)
                    return NotFound(new { message = $"No districts found in region '{region}'" });

                return Ok(districts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving districts: {ex.Message}");
            }
        }

        // GET: api/Districts/regions
        [HttpGet("regions")]
        public IActionResult GetAllRegions()
        {
            var regions = new List<string>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var sql = @"SELECT DISTINCT Region 
                               FROM Districts 
                               WHERE Region IS NOT NULL AND Region <> ''
                               ORDER BY Region";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                regions.Add(reader.GetString(0));
                            }
                        }
                    }
                }

                return Ok(regions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving regions: {ex.Message}");
            }
        }
    }

    public class DistrictDto
    {
        public string DistrictName { get; set; } = string.Empty;
        public string? Region { get; set; }
    }
}
