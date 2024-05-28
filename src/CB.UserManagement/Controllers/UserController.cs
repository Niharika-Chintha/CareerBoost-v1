using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CB.UserManagement.Models;
using CB.UserManagement.ServiceLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CB.UserManagement.Controllers
{

    [ApiController]
    [Route("[controller]")]

    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        public readonly IuserSL _userSL;
        private readonly IConfiguration _config;

        public UserController(IuserSL userSL, IConfiguration configuration, ILogger<UserController> logger)
        {
            _userSL = userSL;
            _logger = logger;
            _config = configuration;
        }

        //creates a new user
        [HttpPost("Create")]
        public async Task<IActionResult> AddUser(User user)
        {
            Response response = await _userSL.AddUser(user);
            if (response.IsSuccess == "true")
            {
                _logger.LogInformation("User created successfully: {UserEmail}", user.Email);
                return Ok(response);
            }
            else
            {
                _logger.LogWarning("User creation failed: {UserEmail}", user.Email);
                return BadRequest(response);
            }
        }


        //Logs in the user by checking whether that candidate is present with that creds or not and responds with jwt token
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            string emailid = loginRequest.Email;
            string password = loginRequest.Password;
            try
            {
                bool isValidLogin = await _userSL.CheckLoginAsync(emailid, password);

                if (isValidLogin)
                {
                    _logger.LogInformation("Login successful for email: {Email}", emailid);
                    try
                    {
                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                        var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
                            _config["Jwt:Issuer"],
                            null,
                            expires: DateTime.Now.AddMinutes(120),
                            signingCredentials: credentials);

                        var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
                        Console.WriteLine(token);
                        _logger.LogInformation("Token generated for email: {Email}", emailid);

                        var response = new LoginResponse
                        {
                            IsSuccess = true,
                            Message = "Login successful",
                            Token = token
                        };

                        return Ok(response);
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions that occur during the token generation process
                        var response = new LoginResponse
                        {
                            IsSuccess = false,
                            Message = "An error occurred while generating the token. Please try again later.",
                            Token = null
                        };

                        // Log the exception details for debugging purposes
                        Console.WriteLine($"Token generation exception: {ex.Message}");

                        return StatusCode(StatusCodes.Status500InternalServerError, response);
                    }
                }
                else
                {
                    var response = new LoginResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid email or password",
                        Token = null
                    };

                    return Unauthorized(response);
                }
            }
            catch (Exception ex)
            {
                // Handle any other exceptions that occur during the login process
                var response = new LoginResponse
                {
                    IsSuccess = false,
                    Message = "An error occurred during login. Please try again later.",
                    Token = null
                };

                // Log the exception details for debugging purposes
                Console.WriteLine($"Login process exception: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }



        [Authorize]
        [HttpGet("ValidateToken")]
        public string GetData()
        {
            return "getting the data";
        }

        [Authorize]
        [HttpDelete("Deactivate")]
        public async Task<IActionResult> DeactivateUser(string email)
        {
            Response response = await _userSL.DeactivateUser(email);
            if (response.IsSuccess == "true")
            {
                _logger.LogInformation("User deactivated successfully: {Email}", email);
                return Ok(response);
            }
            else
            {
                _logger.LogWarning("User deactivation failed: {Email}", email);
                return BadRequest(response);
            }
        }


        [Authorize]
        [HttpPost("GetAll")]
        public async Task<List<User>> GetAllUsers(int isActive)
        {
            _logger.LogInformation("getting all users");
            return await _userSL.GetAllUsers(isActive);
        }





    }
}