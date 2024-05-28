using CB.JobListings.Models;
using CB.JobListings.ServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace CB.JobListings.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly ILogger<JobsController> _logger;
        public readonly IJobService _jobSL;
        private readonly IConfiguration _config;

        public JobsController(IJobService JobService, IConfiguration configuration, ILogger<JobsController> logger)
        {
            _jobSL = JobService;
            _logger = logger;
            _config = configuration;
        }


        [HttpPost("Create")]
        public async Task<IActionResult> AddJob([FromBody] Job job)
        {
            Response response = await _jobSL.AddJobAsync(job);
            if (response.IsSuccess == "true")
            {
                _logger.LogInformation("Job created successfully: {JobDescription}", job.JobDescription);
                return Ok(response);
            }
            else
            {
                _logger.LogWarning("Job creation failed: {JobDescription}", job.JobDescription);
                return BadRequest(response);
            }
        }

        [HttpGet("{jobId}")]
        public async Task<IActionResult> GetJobById(int jobId)
        {
            Job job = await _jobSL.GetJobByIdAsync(jobId);
            if (job != null)
            {
                _logger.LogInformation("Job retrieved successfully: {JobId}", job.Jobid);
                return Ok(job);
            }
            else
            {
                _logger.LogWarning("Job not found: {JobId}", jobId);
                return NotFound(new Response { IsSuccess = "false", Message = "Job not found." });
            }
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllJobs()
        {
            IEnumerable<Job> jobs = await _jobSL.GetAllJobsAsync();
            _logger.LogInformation("All jobs retrieved successfully.");
            return Ok(jobs);
        }

        // [HttpPut("Update")]
        // public async Task<IActionResult> UpdateJob([FromBody] Job job)
        // {
        //     Response response = await _jobSL.UpdateJobAsync(job);
        //     if (response.IsSuccess == "true")
        //     {
        //         _logger.LogInformation("Job updated successfully: {JobId}", job.Jobid);
        //         return Ok(response);
        //     }
        //     else
        //     {
        //         _logger.LogWarning("Job update failed: {JobId}", job.Jobid);
        //         return BadRequest(response);
        //     }
        // }

        [HttpDelete("Delete/{jobId}")]
        public async Task<IActionResult> DeleteJob(int jobId)
        {
            Response response = await _jobSL.DeleteJobAsync(jobId);
            if (response.IsSuccess == "true")
            {
                _logger.LogInformation("Job deleted successfully: {JobId}", jobId);
                return Ok(response);
            }
            else
            {
                _logger.LogWarning("Job deletion failed: {JobId}", jobId);
                return BadRequest(response);
            }
        }




    }
}
