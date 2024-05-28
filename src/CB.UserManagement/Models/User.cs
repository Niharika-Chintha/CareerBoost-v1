using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CB.UserManagement.Models
{
    public class User
    {
        [JsonIgnore]
        public int Userid { get; set; }
        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }

        public string? MobileNumber { get; set; }

        public string? Gender { get; set; }

        [JsonIgnore]
        public int IsActive { get; set; }


    }
    public class Response
    {
        public string? IsSuccess { get; set; }

        public string? Message { get; set; }
    }
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
    }
}
