using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace LandManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NextOfKinsController : ControllerBase
    {
        private readonly string _connectionString;

        public NextOfKinsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        // GET: api/NextOfKins/client/{clientNo}
        [HttpGet("client/{clientNo}")]
        public IActionResult GetNextOfKinsByClient(string clientNo)
        {
            var nextOfKins = new List<object>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = @"SELECT Id, ClientNo, Name, Relationship, Contact 
                           FROM Next_of_Kins 
                           WHERE ClientNo = @clientNo 
                           ORDER BY Name";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@clientNo", clientNo);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nextOfKins.Add(new
                            {
                                id = reader.GetInt32(0),
                                clientNo = reader.GetString(1),
                                name = reader.GetString(2),
                                relationship = reader.GetString(3),
                                contact = reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
                            });
                        }
                    }
                }
            }

            return Ok(nextOfKins);
        }

        // GET: api/NextOfKins/{id}
        [HttpGet("{id}")]
        public IActionResult GetNextOfKin(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = @"SELECT Id, ClientNo, Name, Relationship, Contact 
                           FROM Next_of_Kins 
                           WHERE Id = @id";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var nextOfKin = new
                            {
                                id = reader.GetInt32(0),
                                clientNo = reader.GetString(1),
                                name = reader.GetString(2),
                                relationship = reader.GetString(3),
                                contact = reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
                            };

                            return Ok(nextOfKin);
                        }
                    }
                }
            }

            return NotFound($"Next of Kin with ID {id} not found");
        }

        // POST: api/NextOfKins
        [HttpPost]
        public IActionResult AddNextOfKin([FromBody] NextOfKinDto dto)
        {
            if (string.IsNullOrEmpty(dto.ClientNo))
                return BadRequest("ClientNo is required");

            if (string.IsNullOrEmpty(dto.Name))
                return BadRequest("Name is required");

            if (string.IsNullOrEmpty(dto.Relationship))
                return BadRequest("Relationship is required");

            var formattedContact = FormatPhoneNumber(dto.Contact ?? "");
            if (formattedContact.StartsWith("Invalid"))
                return BadRequest(formattedContact);

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Check if already exists
                    var checkSql = @"SELECT COUNT(*) FROM Next_of_Kins 
                                    WHERE Name = @name AND Contact = @contact AND ClientNo = @clientNo";

                    using (var checkCmd = new SqlCommand(checkSql, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@name", dto.Name);
                        checkCmd.Parameters.AddWithValue("@contact", formattedContact);
                        checkCmd.Parameters.AddWithValue("@clientNo", dto.ClientNo);

                        var count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                            return Conflict("Next of Kin already exists for this client");
                    }

                    // Insert new next of kin
                    var insertSql = @"INSERT INTO Next_of_Kins (ClientNo, Name, Relationship, Contact) 
                                     VALUES (@clientNo, @name, @relationship, @contact)";

                    using (var insertCmd = new SqlCommand(insertSql, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@clientNo", dto.ClientNo);
                        insertCmd.Parameters.AddWithValue("@name", dto.Name);
                        insertCmd.Parameters.AddWithValue("@relationship", dto.Relationship);
                        insertCmd.Parameters.AddWithValue("@contact", formattedContact);

                        insertCmd.ExecuteNonQuery();
                    }
                }

                return CreatedAtAction(nameof(GetNextOfKinsByClient), new { clientNo = dto.ClientNo }, 
                    new { message = "Next of Kin added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding next of kin: {ex.Message}");
            }
        }

        // PUT: api/NextOfKins/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateNextOfKin(int id, [FromBody] NextOfKinDto dto)
        {
            if (string.IsNullOrEmpty(dto.Name))
                return BadRequest("Name is required");

            if (string.IsNullOrEmpty(dto.Relationship))
                return BadRequest("Relationship is required");

            var formattedContact = FormatPhoneNumber(dto.Contact ?? "");
            if (formattedContact.StartsWith("Invalid"))
                return BadRequest(formattedContact);

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var sql = @"UPDATE Next_of_Kins 
                               SET ClientNo = @clientNo, Name = @name, Relationship = @relationship, Contact = @contact 
                               WHERE Id = @id";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@clientNo", dto.ClientNo);
                        command.Parameters.AddWithValue("@name", dto.Name);
                        command.Parameters.AddWithValue("@relationship", dto.Relationship);
                        command.Parameters.AddWithValue("@contact", formattedContact);
                        command.Parameters.AddWithValue("@id", id);

                        var rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                            return NotFound($"Next of Kin with ID {id} not found");
                    }
                }

                return Ok(new { message = "Next of Kin updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating next of kin: {ex.Message}");
            }
        }

        // DELETE: api/NextOfKins/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteNextOfKin(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var sql = @"DELETE FROM Next_of_Kins WHERE Id = @id";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        var rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                            return NotFound($"Next of Kin with ID {id} not found");
                    }
                }

                return Ok(new { message = "Next of Kin deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting next of kin: {ex.Message}");
            }
        }

        private string FormatPhoneNumber(string inputNumber)
        {
            string outputNumber = "";

            // Remove any non-digit characters
            foreach (char c in inputNumber)
            {
                if (char.IsDigit(c))
                {
                    outputNumber += c;
                }
            }

            // Check if the output number is in the desired format (12 digits)
            if (outputNumber.Length == 12)
            {
                return outputNumber;
            }
            else if (outputNumber.Length == 10)
            {
                outputNumber = "265" + outputNumber.Substring(1);
                return outputNumber;
            }
            else
            {
                return "Invalid number format! Please enter a phone number in this format: 265789789789";
            }
        }
    }

    public class NextOfKinDto
    {
        public string? ClientNo { get; set; }
        public string? Name { get; set; }
        public string? Relationship { get; set; }
        public string? Contact { get; set; }
    }
}
