using CB.UserManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace CB.UserManagement.RepositoryLayer
{
    public interface IUserRl
    {
        public Task<int> AddUserAsync(User user);
        public Task<bool> CheckLoginAsync(string email, string password);
        public Task<int> deactivateuser(string email);
        public Task<List<User>> GetAllUsers(int isActive);
    }
}