using CB.JobListings.Models;

namespace CB.JobListings.RepositoryLayer
{
    public interface IJobRepository
    {
        Task<int> AddJobAsync(Job job);
        Task<Job> GetJobByIdAsync(int jobId);
        Task<IEnumerable<Job>> GetAllJobsAsync();
        // Task<int> UpdateJobAsync(Job job);
        Task<int> DeleteJobAsync(int jobId);
    }
}
