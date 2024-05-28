using CB.JobListings.Models;
using CB.JobListings.RepositoryLayer;

namespace CB.JobListings.ServiceLayer
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository JobRepository)
        {
            _jobRepository = JobRepository;
        }

        public async Task<Response> AddJobAsync(Job job)
        {
            Response response = new Response();
            try
            {
                int rowsAffected = await _jobRepository.AddJobAsync(job);
                if (rowsAffected > 0)
                {
                    response.IsSuccess = "true";
                    response.Message = "Job added successfully.";
                }
                else
                {
                    response.IsSuccess = "false";
                    response.Message = "Failed to add job.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = "false";
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Job> GetJobByIdAsync(int jobId)
        {
            return await _jobRepository.GetJobByIdAsync(jobId);
        }

        public async Task<IEnumerable<Job>> GetAllJobsAsync()
        {
            return await _jobRepository.GetAllJobsAsync();
        }

        // public async Task<Response> UpdateJobAsync(Job job)
        // {
        //     Response response = new Response();
        //     try
        //     {
        //         int rowsAffected = await _jobRepository.UpdateJobAsync(job);
        //         if (rowsAffected > 0)
        //         {
        //             response.IsSuccess = "true";
        //             response.Message = "Job updated successfully.";
        //         }
        //         else
        //         {
        //             response.IsSuccess = "false";
        //             response.Message = "Failed to update job.";
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         response.IsSuccess = "false";
        //         response.Message = ex.Message;
        //     }
        //     return response;
        // }

        public async Task<Response> DeleteJobAsync(int jobId)
        {
            Response response = new Response();
            try
            {
                int rowsAffected = await _jobRepository.DeleteJobAsync(jobId);
                if (rowsAffected > 0)
                {
                    response.IsSuccess = "true";
                    response.Message = "Job deleted successfully.";
                }
                else
                {
                    response.IsSuccess = "false";
                    response.Message = "Failed to delete job.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = "false";
                response.Message = ex.Message;
            }
            return response;
        }
    }
}