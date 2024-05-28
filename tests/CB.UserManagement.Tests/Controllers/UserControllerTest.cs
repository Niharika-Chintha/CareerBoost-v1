using System;
using System.Threading.Tasks;
using CB.UserManagement.Controllers;
using CB.UserManagement.Models;
using CB.UserManagement.ServiceLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CB.UserManagement.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<IuserSL> _mockUserSL;
        private readonly UserController _userController;
        private readonly Mock<ILogger<UserController>> _mockLogger;

        public UserControllerTests()
        {
            _mockConfig = new Mock<IConfiguration>();
            _mockUserSL = new Mock<IuserSL>();
            _userController = new UserController(_mockUserSL.Object, _mockConfig.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task AddUser_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var user = new User
            {
                Userid = 1,
                UserName = "JohnDoe",
                Password = "StrongPassword123!",
                Email = "john.doe@example.com",
                MobileNumber = "1234567890",
                Gender = "Male"
            };
            var response = new Response { IsSuccess = "true" };
            _mockUserSL.Setup(x => x.AddUser(user)).ReturnsAsync(response);

            // Act
            var result = await _userController.AddUser(user) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        // [Fact]
        // public async Task AddUser_InvalidInput_ReturnsBadRequestResult()
        // {
        //     // Arrange
        //     var user = new User
        //     {
        //         Userid = 2, // Assuming this ID is invalid
        //         UserName = "", // Empty username
        //         Password = "", // Weak password
        //         Email = "invalid_email", // Invalid email format
        //         MobileNumber = "", // Empty mobile number
        //         Gender = "Other" // Invalid gender };
        //     };

        //     // Act
        //     var result = await _userController.AddUser(user) as BadRequestObjectResult;

        //     // Assert
        //     Assert.NotNull(result);
        //     Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        // }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "test@gmail.com",
                Password = "password123"
            };
            var isValidLogin = true;
            _mockUserSL.Setup(x => x.CheckLoginAsync(loginRequest.Email, loginRequest.Password)).ReturnsAsync(isValidLogin);
            _mockConfig.SetupGet(x => x["Jwt:Key"]).Returns("test_secret_key");
            _mockConfig.SetupGet(x => x["Jwt:Issuer"]).Returns("test_issuer");

            // Act
            var result = await _userController.Login(loginRequest) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }



        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorizedResult()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "test@gmail.com",
                Password = "password123"
            };
            var isValidLogin = false;
            _mockUserSL.Setup(x => x.CheckLoginAsync(loginRequest.Email, loginRequest.Password)).ReturnsAsync(isValidLogin);

            // Act
            var result = await _userController.Login(loginRequest) as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
        }


    }
}
