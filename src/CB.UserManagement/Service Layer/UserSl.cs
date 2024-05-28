
using CB.UserManagement.Models;
using CB.UserManagement.RepositoryLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CB.UserManagement.ServiceLayer
{
    public class UserSL : IuserSL
    {
        private readonly IUserRl _userRL;
        private readonly ILogger<UserSL> _logger;

        public UserSL(IUserRl userRL,ILogger<UserSL> logger)
        {
            _userRL = userRL;
             _logger = logger;
        }

        public async Task<Response> AddUser(User user)
        {
            Response response = new Response();
            try
            {
                int rowsAffected = await _userRL.AddUserAsync(user);
                if (rowsAffected > 0)
                {
                    response.IsSuccess = "true";
                    response.Message = "User added successfully.";
                }
                else
                {
                    response.IsSuccess = "false";
                    response.Message = "Failed to add user.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = "false";
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<bool> CheckLoginAsync(string email, string password)
        {
            return await _userRL.CheckLoginAsync(email, password);
        }

        private static IActionResult NotFound()
        {
            return new NotFoundResult();
        }

        private static IActionResult Ok(string token)
        {
            return new OkObjectResult(token);
        }

        public async Task<Response> DeactivateUser(string email)
        {
            Response response = new Response();
            try
            {
                int rowsAffected = await _userRL.deactivateuser(email);
                if (rowsAffected > 0)
                {
                    response.IsSuccess = "true";
                    response.Message = "User deactivated successfully.";
                }
                else
                {
                    response.IsSuccess = "false";
                    response.Message = "Failed to deactivate user.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = "false";
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<List<User>> GetAllUsers(int isActive)
        {
            return await _userRL.GetAllUsers(isActive);
        }
    }
}

