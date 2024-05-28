using System.Data;
using CB.UserManagement.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Microsoft.Extensions.Logging;

namespace CB.UserManagement.RepositoryLayer
{
    public class UserRl : IUserRl
    {
        private readonly string _connectionString;
        private readonly ILogger<UserRl> _logger;

        public UserRl(IConfiguration configuration, ILogger<UserRl> logger)
        {
            _connectionString = configuration.GetConnectionString("MySqlConnection");
            _logger = logger;
        }

        public async Task<int> AddUserAsync(User user)
        {
            try
            {
                // Validate the user input
                if (!ValidationHelper.IsValidMobileNumber(user.MobileNumber))
                {
                    throw new ArgumentException("Mobile number should have 10 digits.");
                }

                if (!ValidationHelper.IsValidEmail(user.Email))
                {
                    throw new ArgumentException("Invalid email format.");
                }

                if (!ValidationHelper.IsValidPassword(user.Password))
                {
                    throw new ArgumentException("Password must meet the password policy.");
                }

                // Attempt to add the user to the database
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = "INSERT INTO Users (UserName, Password, EmailId, MobileNumber, Gender) VALUES (@Name, @Password, @Email, @MobileNum, @Gender)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", user.UserName);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@MobileNum", user.MobileNumber);
                        command.Parameters.AddWithValue("@Gender", user.Gender);

                        return await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }


        public async Task<bool> CheckLoginAsync(string email, string password)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = "SELECT * FROM Users WHERE EmailId = @Email AND Password = @Password";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            return reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task<int> deactivateuser(string email) 
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = "UPDATE users SET IsActive = 0 WHERE EmailId = @EmailId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmailId", email);

                        return await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task<List<User>> GetAllUsers(int isActive)
        {
            var users = new List<User>();
            const string query = "SELECT * FROM users WHERE IsActive = @IsActive";
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IsActive", isActive);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var user = new User
                                {
                                    Userid = reader.GetInt32("UserId"),
                                    UserName = reader.IsDBNull("UserName") ? null : reader.GetString("UserName"),
                                    Email = reader.IsDBNull("EmailId") ? null : reader.GetString("EmailId"),
                                    MobileNumber = reader.IsDBNull("MobileNumber") ? null : reader.GetString("MobileNumber"),
                                    Gender = reader.IsDBNull("Gender") ? null : reader.GetString("Gender"),
                                    IsActive = reader.GetInt32("IsActive")
                                };
                                users.Add(user);
                            }
                        }
                    }
                }

                return users;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }


    }
}
