using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webscraper_WPF_Maarten_Hens_R0739214_DAL.DomainModels
{
    public class YoutubeVideo
    {
        public YoutubeVideo(string? title, string? uploader, string? views, string? videoUrl)
        {
            Title = title;
            Uploader = uploader;
            Views = views;
            VideoUrl = videoUrl;
        }
        public string? Title { get; set; }
        public string? Uploader { get; set; }

        public string? Views { get; set; }

        public string? VideoUrl { get; set; }
    }
}
