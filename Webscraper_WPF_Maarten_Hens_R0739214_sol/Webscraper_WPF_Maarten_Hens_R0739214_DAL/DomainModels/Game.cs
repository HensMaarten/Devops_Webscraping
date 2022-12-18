using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webscraper_WPF_Maarten_Hens_R0739214_DAL.DomainModels
{
    public class Game
    {
        public Game(string? title, string? webshop, string? platform, double? price, string? gameUrl)
        {
            Title = title;
            Webshop = webshop;
            Platform = platform;
            Price = price;
            GameUrl = gameUrl; 
        }

        public string? Title { get; set; }

        public string? Webshop { get; set; }
        public string? Platform { get; set; }

        public double? Price { get; set; }

        public string? GameUrl { get; set; } 
    }
}
