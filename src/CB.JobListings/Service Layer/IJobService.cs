using CB.JobListings.Models;

namespace CB.JobListings.ServiceLayer
{
    public interface IJobService
    {
        Task<Response> AddJobAsync(Job job);
        Task<Job> GetJobByIdAsync(int jobId);
        Task<IEnumerable<Job>> GetAllJobsAsync();
        // Task<Response> UpdateJobAsync(Job job);
        Task<Response> DeleteJobAsync(int jobId);
    }

}