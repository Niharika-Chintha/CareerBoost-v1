using System.Text.Json.Serialization;

namespace CB.JobListings.Models
{
    public class Job
    {
        [JsonIgnore]
        public int Jobid { get; set; }
        public string? JobDescription { get; set; }

        public string? ExpectedExperience { get; set; }

        public string? Skillset { get; set; }

        public string? Locations { get; set; }

        [JsonIgnore]
        public int IsActive { get; set; }


    }
    public class Response
    {
        public string? IsSuccess { get; set; }

        public string? Message { get; set; }
    }
}