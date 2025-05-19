using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CarPartsShop.Controllers
{
    [ApiController]
    [Route("api")]
    public class PartsController : ControllerBase
    {
        private readonly string _connectionString;

        public PartsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        }

        [HttpGet("customers/search")]
        public async Task<IActionResult> SearchCustomers([FromQuery] string name)
        {
            var results = new List<Dictionary<string, object>>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                // Vulnerable query
                var query = $"SELECT * FROM Customers WHERE name = '{name}'";
                using var command = new SqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[reader.GetName(i)] = reader.GetValue(i);
                    }
                    results.Add(row);
                }
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("customers/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                // Vulnerable query
                var query = $"SELECT * FROM Customers WHERE phone_number = '{request.PhoneNumber}' AND shop_id = 1";
                using var command = new SqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return Ok(new { message = "Login successful", customer_id = reader["id"] });
                }
                return Unauthorized(new { message = "Invalid credentials" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    public class LoginRequest
    {
        public string PhoneNumber { get; set; }
    }
}