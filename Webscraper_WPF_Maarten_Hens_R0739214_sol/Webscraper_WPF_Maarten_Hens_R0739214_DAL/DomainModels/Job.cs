using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webscraper_WPF_Maarten_Hens_R0739214_DAL.DomainModels
{
    public class Job
    {
        public Job(string? title, string? company, string? keywords, string? location, string? jobUrl)
        {
            Title = title;
            Company = company;
            Keywords = keywords;
            Location = location;
            JobUrl = jobUrl;
        }

        public string? Title { get; set; }
        public string? Company { get; set; }

        public string? Keywords { get; set; }

        public string? Location { get; set; }

        public string? JobUrl { get; set; }
    }
}
