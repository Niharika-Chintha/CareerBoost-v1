
using CB.UserManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace CB.UserManagement.ServiceLayer
{
    public interface IuserSL
    {
        public Task<Response> AddUser(User user);
        public Task<bool> CheckLoginAsync(string email, string password);

        public Task<Response> DeactivateUser(string email);
        public Task<List<User>> GetAllUsers(int isActive);
    }
}