using CB.JobListings.Models;
using CB.JobListings.RepositoryLayer;
using MySqlConnector;

public class JobRepository : IJobRepository
{
    private readonly string _connectionString;

    public JobRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MySqlConnection");
    }

    public async Task<int> AddJobAsync(Job job)
    {
        try
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "INSERT INTO Jobs (JobDescription, ExpectedExperience, Skillset, Locations) VALUES (@JobDescription, @ExpectedExperience, @Skillset, @Location)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@JobDescription", job.JobDescription);
                    command.Parameters.AddWithValue("@ExpectedExperience", job.ExpectedExperience);
                    command.Parameters.AddWithValue("@Skillset", job.Skillset);
                    command.Parameters.AddWithValue("@Location", job.Locations);

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

    public async Task<Job> GetJobByIdAsync(int jobId)
    {
        try
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM Jobs WHERE JobId = @JobId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@JobId", jobId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new Job
                            {
                                Jobid = reader.GetInt32("JobId"),
                                JobDescription = reader.GetString("JobDescription"),
                                ExpectedExperience = reader.GetString("ExpectedExperience"),
                                Skillset = reader.GetString("Skillset"),
                                Locations = reader.GetString("Locations")
                            };
                        }
                        return null;
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

    public async Task<IEnumerable<Job>> GetAllJobsAsync()
    {
        try
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM Jobs";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var jobs = new List<Job>();
                        while (reader.Read())
                        {
                            jobs.Add(new Job
                            {
                                Jobid = reader.GetInt32("JobId"),
                                JobDescription = reader.GetString("JobDescription"),
                                ExpectedExperience = reader.GetString("ExpectedExperience"),
                                Skillset = reader.GetString("Skillset"),
                                Locations = reader.GetString("Locations")
                            });
                        }
                        return jobs;
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

    // public async Task<int> UpdateJobAsync(Job job)
    // {
    //     try
    //     {
    //         using (var connection = new MySqlConnection(_connectionString))
    //         {
    //             await connection.OpenAsync();

    //             string query = "UPDATE Jobs SET JobDescription = @JobDescription, ExpectedExperience = @ExpectedExperience, Skillset = @Skillset, Locations = @Location WHERE JobId = @JobId";
    //             using (var command = new MySqlCommand(query, connection))
    //             {
    //                 command.Parameters.AddWithValue("@JobDescription", job.JobDescription);
    //                 command.Parameters.AddWithValue("@ExpectedExperience", job.ExpectedExperience);
    //                 command.Parameters.AddWithValue("@Skillset", job.Skillset);
    //                 command.Parameters.AddWithValue("@Location", job.Locations);
    //                 command.Parameters.AddWithValue("@JobId", job.Jobid);

    //                 return await command.ExecuteNonQueryAsync();
    //             }
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.Error.WriteLine($"An error occurred: {ex.Message}");
    //         throw;
    //     }
    // }

    public async Task<int> DeleteJobAsync(int jobId)
    {
        try
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE Jobs SET IsActive = 0 WHERE JobId= @jobId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@jobId", jobId);

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


}
