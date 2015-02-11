using Newtonsoft.Json;
using System.Collections.Generic;
using TinderApp.DbHelper;

namespace Tinder4Jobs.Model
{
    public class LinkedinJob
    {
        DatabaseLinkedinJobHelperClass dbHelper = new DatabaseLinkedinJobHelperClass();

        [JsonProperty("company")]
        public LinkedinCompany Company { get; set; }

        [JsonProperty("descriptionSnippet")]
        public string DescriptionSnippet { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("jobPoster")]
        public LinkedinJobPoster JobPoster { get; set; }

        [JsonProperty("locationDescription")]
        public string LocationDescription { get; set; }

        public List<LinkedinJob> GetAllJobsByStatus(string status)
        {
            List<LinkedinJob> jobList = new List<LinkedinJob>();

            var jobs = dbHelper.ReadAllJobsByStatus(status);

            foreach (var item in jobs)
            {
                LinkedinJob linkedinJob = new LinkedinJob();

                linkedinJob.Company = new LinkedinCompany();
                linkedinJob.Company.Id = item.CompanyId;
                linkedinJob.Company.Name = item.CompanyName;
                linkedinJob.DescriptionSnippet = item.DescriptionSnippet;
                linkedinJob.Id = item.JobId.ToString();

                linkedinJob.JobPoster = new LinkedinJobPoster();
                linkedinJob.JobPoster.FirstName = item.JobPosterFirstName;
                //linkedinJob.JobPoster.LastName = item..JobPosterFirstName;

                linkedinJob.LocationDescription = item.LocationDescription;
                
                jobList.Add(linkedinJob);
            }

            return jobList;

        }

        public LinkedinJob GetJobById(string jobId)
        {
            var job = dbHelper.ReadJob(jobId);

            LinkedinJob linkedinJob = new LinkedinJob();

            linkedinJob.Company = new LinkedinCompany();
            linkedinJob.Company.Id = job.CompanyId;
            linkedinJob.Company.Name = job.CompanyName;
            linkedinJob.DescriptionSnippet = job.DescriptionSnippet;
            linkedinJob.Id = job.JobId.ToString();

            linkedinJob.JobPoster = new LinkedinJobPoster();
            linkedinJob.JobPoster.FirstName = job.JobPosterFirstName;
            //linkedinJob.JobPoster.LastName = item..JobPosterFirstName;

            linkedinJob.LocationDescription = job.LocationDescription;

            return linkedinJob;

        }

    }

    public class LinkedinCompany
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

    }

    public class LinkedinJobPoster
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("headline")]
        public string Headline { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }

    public class JobList
    {
        [JsonProperty("_total")]
        public string Total { get; set; }

        [JsonProperty("values")]
        public LinkedinJob[] Values { get; set; }

    }

    public class Facets
    {
        [JsonProperty("_total")]
        public string Total { get; set; }
    }

    public class LinkedinJobList
    {
        [JsonProperty("facets")]
        public Facets Facets { get; set; }

        [JsonProperty("jobs")]
        public JobList Jobs { get; set; }

        [JsonProperty("numResults")]
        public string NumResults { get; set; }



    }
}