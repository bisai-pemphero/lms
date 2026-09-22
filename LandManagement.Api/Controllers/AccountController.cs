using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LandManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly string _connectionString;
        private const string EncryptionKey = "06061982";

        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        // GET: api/Account/profile
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value ?? User.FindFirst("username")?.Value;
            
            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var sql = @"SELECT Username, Fullname, Location, RoleId, Email, PhoneNumber 
                               FROM Users 
                               WHERE Username = @username";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var profile = new
                                {
                                    username = reader.GetString(0),
                                    fullname = reader.GetString(1),
                                    location = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    roleId = reader.GetInt32(3),
                                    email = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                    phoneNumber = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                                };

                                return Ok(profile);
                            }
                        }
                    }

                    return NotFound(new { message = "User profile not found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving profile: {ex.Message}");
            }
        }

        // PUT: api/Account/change-password
        [HttpPut("change-password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value ?? User.FindFirst("username")?.Value;
            
            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "User not authenticated" });

            if (string.IsNullOrEmpty(dto.CurrentPassword))
                return BadRequest(new { message = "Current password is required" });

            if (string.IsNullOrEmpty(dto.NewPassword))
                return BadRequest(new { message = "New password is required" });

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Get current password from database
                    var getCurrentPasswordSql = @"SELECT Password FROM Users WHERE Username = @username";
                    string? currentPasswordHash = null;

                    using (var cmd = new SqlCommand(getCurrentPasswordSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                currentPasswordHash = reader.GetString(0);
                            }
                        }
                    }

                    if (currentPasswordHash == null)
                        return NotFound(new { message = "User not found" });

                    // Encrypt the provided current password to compare
                    var providedCurrentPasswordHash = Encrypt(dto.CurrentPassword, true);

                    if (providedCurrentPasswordHash != currentPasswordHash)
                        return BadRequest(new { message = "Current password is incorrect" });

                    // Update password
                    var newPasswordHash = Encrypt(dto.NewPassword, true);
                    var updateSql = @"UPDATE Users SET Password = @password WHERE Username = @username";

                    using (var cmd = new SqlCommand(updateSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@password", newPasswordHash);
                        cmd.Parameters.AddWithValue("@username", username);

                        var rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                            return NotFound(new { message = "User not found" });
                    }

                    return Ok(new { message = "Password changed successfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error changing password: {ex.Message}");
            }
        }

        // POST: api/Account/reset-password (Admin function)
        [HttpPost("reset-password")]
        [Authorize(Roles = "Admin")] // Only admins can reset passwords
        public IActionResult ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (string.IsNullOrEmpty(dto.Username))
                return BadRequest(new { message = "Username is required" });

            if (string.IsNullOrEmpty(dto.NewPassword))
                return BadRequest(new { message = "New password is required" });

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Check if user exists
                    var checkUserSql = @"SELECT COUNT(*) FROM Users WHERE Username = @username";
                    int userCount;

                    using (var cmd = new SqlCommand(checkUserSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", dto.Username);
                        userCount = (int)cmd.ExecuteScalar();
                    }

                    if (userCount == 0)
                        return NotFound(new { message = "User not found" });

                    // Reset password
                    var newPasswordHash = Encrypt(dto.NewPassword, true);
                    var updateSql = @"UPDATE Users SET Password = @password WHERE Username = @username";

                    using (var cmd = new SqlCommand(updateSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@password", newPasswordHash);
                        cmd.Parameters.AddWithValue("@username", dto.Username);

                        var rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                            return NotFound(new { message = "User not found" });
                    }

                    return Ok(new { message = $"Password reset successfully for user {dto.Username}" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error resetting password: {ex.Message}");
            }
        }

        // PUT: api/Account/update-profile
        [HttpPut("update-profile")]
        public IActionResult UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value ?? User.FindFirst("username")?.Value;
            
            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "User not authenticated" });

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var updateSql = @"UPDATE Users 
                                     SET Fullname = @fullname, 
                                         Location = @location,
                                         Email = @email,
                                         PhoneNumber = @phoneNumber
                                     WHERE Username = @username";

                    using (var cmd = new SqlCommand(updateSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@fullname", dto.Fullname ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@location", dto.Location ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(dto.Email) ? (object)DBNull.Value : dto.Email);
                        cmd.Parameters.AddWithValue("@phoneNumber", string.IsNullOrEmpty(dto.PhoneNumber) ? (object)DBNull.Value : dto.PhoneNumber);
                        cmd.Parameters.AddWithValue("@username", username);

                        var rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                            return NotFound(new { message = "User not found" });
                    }

                    return Ok(new { message = "Profile updated successfully" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating profile: {ex.Message}");
            }
        }

        // Helper method to encrypt password (matching legacy code)
        private static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            string key = EncryptionKey;

            if (useHashing)
            {
                using (var hashmd5 = MD5.Create())
                {
                    keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                }
            }
            else
            {
                keyArray = Encoding.UTF8.GetBytes(key);
            }

            using (var tdes = new TripleDESCryptoServiceProvider())
            {
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                var cTransform = tdes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
        }
    }

    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public class ResetPasswordDto
    {
        public string Username { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public class UpdateProfileDto
    {
        public string? Fullname { get; set; }
        public string? Location { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
