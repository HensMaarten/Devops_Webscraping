
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Webscraper_WPF_Maarten_Hens_R0739214_DAL.DomainModels;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using OpenQA.Selenium.Interactions;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace Webscraper_WPF_Maarten_Hens_R0739214.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region properties
        //public ICommand UpdateViewCommand { get; set; }

        public IWebDriver driver { get; set; }

        public ChromeOptions ChromeOptions { get; set; }

        public DialogViewModel? DialogViewModel { get; set; }

        public ChromeDriverService ChromeDriverService { get; set; }
        public string IntroText { get; set; }

        public string HomeContentVisible { get; set; }
        public string YoutubeContentVisible { get; set; }
        public string IctJobsContentVisible { get; set; }
        public string GamePricesContentVisible { get; set; }

        public Game? SelectedGame
        {
            get { return _selectedGame; }
            set
            {
                _selectedGame = value;
                NotifyPropertyChanged();
            }
        }

        public Job? SelectedJob
        {
            get { return _selectedJob; }
            set
            {
                _selectedJob = value;
                NotifyPropertyChanged();
            }
        }
        public YoutubeVideo? SelectedYoutubeVideo
        {
            get { return _selectedYoutubeVideo; }
            set
            {
                _selectedYoutubeVideo = value;
                NotifyPropertyChanged();
            }
        }

        private Game? _selectedGame;
        private Job? _selectedJob;
        private YoutubeVideo? _selectedYoutubeVideo;
        private string? _gameSearchTerm;
        private string? _ictJobsSearchTerm;
        private string? _youtubeSearchTerm;

        public string? GameSearchTerm
        {
            get { return _gameSearchTerm; }
            set
            {
                _gameSearchTerm = value;
                NotifyPropertyChanged();
            }
        }
        public string? IctJobsSearchTerm
        {
            get { return _ictJobsSearchTerm; }
            set
            {
                _ictJobsSearchTerm = value;
                NotifyPropertyChanged();
            }
        }
        public string? YoutubeSearchTerm
        {
            get { return _youtubeSearchTerm; }
            set
            {
                _youtubeSearchTerm = value;
                NotifyPropertyChanged();
            }
        }


        #endregion


        #region lists
        // private lists 
        private ObservableCollection<YoutubeVideo>? _youtubeVideos;
        private ObservableCollection<Job>? _jobs;
        private ObservableCollection<Game> _games;

        // public lists
        public ObservableCollection<YoutubeVideo>? YoutubeVideos
        {
            get { return _youtubeVideos; }
            set
            {
                _youtubeVideos = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Job>? Jobs
        {
            get { return _jobs; }
            set
            {
                _jobs = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Game> Games
        {
            get { return _games; }
            set
            {
                _games = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        public MainViewModel()
        {
            IntroText = "Welkom bij deze applicatie. Met deze software kan je info van verschillende websites ophalen.\nKlik bovenaan op de knoppen Youtube, ICTJobs of Game prices om info van deze websites op te halen.";
            //UpdateViewCommand = new UpdateViewCommand(this);
            HomeContentVisible = "Visible";
            YoutubeContentVisible = "Collapsed";
            IctJobsContentVisible = "Collapsed";
            GamePricesContentVisible = "Collapsed";
            ChromeOptions = new ChromeOptions();
            ChromeOptions.AddArguments("headless");
            ChromeDriverService = ChromeDriverService.CreateDefaultService();
            ChromeDriverService.HideCommandPromptWindow = true;
            _games = new ObservableCollection<Game>();
            driver = new ChromeDriver(ChromeDriverService, ChromeOptions);
        }

        public override string this[string columnName]
        {
            get { return ""; }
        }

        public override bool CanExecute(object? parameter)
        {
            if (parameter != null)
            {
                if (parameter.ToString() == "SortPricesAscending")
                {
                    return Games.Count() > 0 ? true : false;
                }
                else if (parameter.ToString() == "SortPricesDescending")
                {
                    return Games.Count() > 0 ? true : false;
                }
                else if (parameter.ToString() == "CopyGameUrl")
                {
                    return SelectedGame != null ? true : false;
                }
                else if (parameter.ToString() == "CopyJobUrl")
                {
                    return SelectedJob != null ? true : false;
                }
                else if (parameter.ToString() == "CopyYoutubeUrl")
                {
                    return SelectedYoutubeVideo != null ? true : false;                
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        public override void Execute(object? parameter)
        {
            if (parameter != null)
            {
                if (parameter.ToString() == "ShowHomeContent")
                {
                    HandleContentChange(true, false, false, false);
                }
                else if (parameter.ToString() == "ShowYoutubeContent")
                {
                    HandleContentChange(false, true, false, false);
                }
                else if (parameter.ToString() == "ShowIctJobsContent")
                {
                    HandleContentChange(false, false, true, false);
                }
                else if (parameter.ToString() == "ShowGamePricesContent")
                {
                    HandleContentChange(false, false, false, true);
                }
                else if (parameter.ToString() == "ScrapeYoutube")
                {
                    if(YoutubeSearchTerm != null)
                    {
                        ScrapeYoutube();
                    }
                    else ShowErrorMessage();
                }
                else if (parameter.ToString() == "ScrapeICTJobs")
                {
                    if (IctJobsSearchTerm != null)
                    {
                        ScrapeICTJobs();
                    }
                    else ShowErrorMessage();
                }
                else if (parameter.ToString() == "ScrapeGameSites")
                {
                    if (GameSearchTerm != null)
                    {
                        GameScraping();
                    }
                    else ShowErrorMessage();
                }
                else if (parameter.ToString() == "SortPricesAscending")
                {
                    SortGamesByPrice("A");
                }
                else if (parameter.ToString() == "SortPricesDescending")
                {
                    SortGamesByPrice("D");
                }
                else if (parameter.ToString() == "CopyGameUrl")
                {
                    CopyToClipboard("game");
                }
                else if (parameter.ToString() == "CopyJobUrl") 
                {
                    CopyToClipboard("job");
                }
                else if (parameter.ToString() == "CopyYoutubeUrl")
                {
                    CopyToClipboard("youtube");
                }
            }
        }

        public void HandleContentChange(bool home, bool youtube, bool ictJobs, bool gamePrices)
        {
            HomeContentVisible = home == true ? "Visible" : "Collapsed";
            YoutubeContentVisible = youtube == true ? "Visible" : "Collapsed";
            IctJobsContentVisible = ictJobs == true ? "Visible" : "Collapsed";
            GamePricesContentVisible = gamePrices == true ? "Visible" : "Collapsed";
        }

        public void ShowErrorMessage()
        {
            DialogViewModel = new DialogViewModel("Error", "De webscraper heeft een zoekterm nodig voor deze kan beginnen met scrapen");
            DialogViewModel.ShowDialog();
        }
        public void ShowMessage(string title, string message)
        {
            DialogViewModel = new DialogViewModel(title, message);
            DialogViewModel.ShowDialog();
        }
        public void ScrapeYoutube()
        {
            YoutubeVideos = new ObservableCollection<YoutubeVideo>();
            driver = new ChromeDriver(ChromeDriverService, ChromeOptions);       
            using (driver)
            {
                driver.Navigate().GoToUrl("https://www.youtube.com/results?search_query=" + YoutubeSearchTerm + "&sp=CAI%253D");
                var timeout = 10000; /* Maximum wait time of 10 seconds */
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
                wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
                Thread.Sleep(3000);
                driver.FindElement(By.CssSelector(".yt-spec-button-shape-next.yt-spec-button-shape-next--filled.yt-spec-button-shape-next--call-to-action.yt-spec-button-shape-next--size-m ")).Click();
                Thread.Sleep(3000);

                var titleElements = driver.FindElements(By.XPath("//*[@id=\"video-title\"]"));
                var uploadDivs = driver.FindElements(By.XPath("//div[@id=\"channel-info\"]/a"));
                var viewDivs = driver.FindElements(By.CssSelector("span.inline-metadata-item.style-scope.ytd-video-meta-block"));

                int amount_of_videos = 5;
                int x = 0;
                for (int i = 0; i < amount_of_videos; i++)
                {
                    var titleElement = titleElements.ElementAt(i);
                    var uploadDiv = uploadDivs.ElementAt(i);
                    var uploadLink = uploadDiv.GetAttribute("href");
                    var uploader = uploadLink.Substring(uploadLink.LastIndexOf('@') + 1);
                    var title = titleElement.GetAttribute("title");
                    var link = titleElement.GetAttribute("href");
                    string views = viewDivs.ElementAt(x).Text;
                    if (views.Contains("Gepland") || views.Contains("première"))
                    {
                        views = "0 weergaven";
                        x++;
                    }
                    else
                    {
                        x = x + 2;
                    }
                    YoutubeVideo youtubeVideo = new YoutubeVideo(title, uploader, views, link);
                    YoutubeVideos.Add(youtubeVideo);
                }
            }
            driver.Quit();
            WriteToJson(YoutubeVideos);
        }

        public void ScrapeICTJobs()
        {
            Jobs = new ObservableCollection<Job>();
            driver = new ChromeDriver(ChromeDriverService, ChromeOptions);
            using (driver)
            {
                driver.Navigate().GoToUrl("https://www.ictjob.be/nl/it-vacatures-zoeken?keywords=" + IctJobsSearchTerm);
                var timeout = 10000; /* Maximum wait time of 10 seconds */
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
                wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));


                Thread.Sleep(16000);
                driver.FindElement(By.CssSelector("a.close-context-message.close-layer-button")).Click();
                Thread.Sleep(1600);
                driver.FindElement(By.CssSelector("#sort-by-date")).Click();
                wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
                Thread.Sleep(16000);
                var jobElements = driver.FindElements(By.CssSelector("li.search-item.clearfix"));

                int amount_of_jobs = 5;
                for (int i = 0; i < amount_of_jobs; i++)
                {
                    var jobElement = jobElements.ElementAt(i);
                    var jobTitle = jobElement.FindElement(By.CssSelector("h2.job-title")).Text;
                    var jobCompany = jobElement.FindElement(By.CssSelector("span.job-company")).Text;
                    var keywords = jobElement.FindElements(By.XPath("//span[contains(@itemprop, 'description')]")).ElementAt(i).Text;
                    var location = jobElement.FindElements(By.XPath("//span[contains(@itemprop, 'addressLocality')]")).ElementAt(i).Text;
                    var jobLink = jobElement.FindElement(By.CssSelector(".job-title.search-item-link")).GetAttribute("href");
                    Job job = new Job(jobTitle, jobCompany, keywords, location, jobLink);
                    Jobs.Add(job);
                }
            }
            driver.Quit();
            WriteToJson(Jobs);
        }
        public void GameScraping()
        {
            Games = new ObservableCollection<Game>();
            NedgameScraping();
            BolScraping();
            GameManiaScraping();
            driver.Quit();
            WriteToJson(Games);
        }

        public void GameManiaScraping()
        {
            // doesn't work in headless mode
            driver = new ChromeDriver(ChromeDriverService);
            using (driver)
            {
                driver.Navigate().GoToUrl("https://www.gamemania.be/nl/product/search?keyword=" + GameSearchTerm);
                var timeout = 20000; /* Maximum wait time of 10 seconds */
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
                wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
                Thread.Sleep(3000);
                driver.FindElement(By.CssSelector("#cookiescript_accept")).Click();

                var gameElements = driver.FindElements(By.CssSelector("li.span12.clearfix.thumb.thumbListerView.listView"));
                int amount_of_games = gameElements.Count();
                for (int i = 0; i < amount_of_games; i++)
                {
                    IWebElement gameElement = gameElements.ElementAt(i);
                    var gameTitleDiv = gameElement.FindElement(By.CssSelector("div.thumbTitle"));
                    var gameTitle = gameTitleDiv.FindElement(By.CssSelector("a")).GetAttribute("innerText");
                    var gamePlatformDiv = gameElement.FindElement(By.CssSelector("div.thumbBrand"));
                    var gamePlatform = gamePlatformDiv.FindElement(By.CssSelector("a")).GetAttribute("innerText");

                    var gamePriceString = GameManiaPriceScraping(gameElement);
                    gamePriceString = gamePriceString.Replace(",", ".");
                    gamePriceString = gamePriceString.Replace("New", "");
                    gamePriceString = gamePriceString.Replace("€", "");
                    gamePriceString = gamePriceString.Trim();
                    double gamePrice = double.Parse(gamePriceString);
                    var gameUrl = gameTitleDiv.FindElement(By.CssSelector("a")).GetAttribute("href"); 
                    Game game = new Game(gameTitle, "Game Mania", gamePlatform, gamePrice, gameUrl);
                    Games.Add(game);
                }
            }
        }
        public string GameManiaPriceScraping(IWebElement gameElement)
        {
            var gamePriceDiv = gameElement.FindElement(By.CssSelector("div.thumbPrice"));
            var gamePriceSpans = gamePriceDiv.FindElements(By.CssSelector("span"));
            foreach (var gamePriceSpan in gamePriceSpans)
            {
                bool hasStrikeThroughClass = false;
                string classes = gamePriceSpan.GetAttribute("class");
                if (classes.Contains("strike-through") || classes.Contains("newSpan"))
                {
                    hasStrikeThroughClass = true;
                }
                if (!hasStrikeThroughClass)
                {
                    return gamePriceSpan.GetAttribute("innerText");
                }
            }
            return "";
        }
        public void NedgameScraping()
        {
            driver = new ChromeDriver(ChromeDriverService, ChromeOptions);
            using (driver)
            {
                driver.Navigate().GoToUrl("https://www.nedgame.nl/zoek/zoek:" + GameSearchTerm);
                var timeout = 20000; /* Maximum wait time of 10 seconds */
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
                wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
                Thread.Sleep(3000);

                var gameElements = driver.FindElements(By.CssSelector("div.productShopHeader"));
                int amount_of_games = gameElements.Count();
                for (int i = 0; i < amount_of_games; i++)
                {
                    try
                    {
                    var gameElement = gameElements.ElementAt(i);
                    var gameTitleDiv = gameElement.FindElement(By.CssSelector("a.productTitleLink"));
                    var gameTitle = gameTitleDiv.FindElement(By.CssSelector("h3")).GetAttribute("innerText");
                    var gamePlatformDiv = gameElement.FindElement(By.CssSelector("div.categoryTitle"));
                    var gamePlatform = gamePlatformDiv.FindElement(By.CssSelector("a")).GetAttribute("innerText");
                    var gamePriceString = gameElement.FindElement(By.CssSelector("div.currentprice")).GetAttribute("innerText");
                    gamePriceString = gamePriceString.Replace(",", ".");
                    gamePriceString = gamePriceString.Replace("-", "");
                    double gamePrice = double.Parse(gamePriceString);
                    var gameImageDiv = gameElement.FindElement(By.CssSelector("div.image"));
                    var gameUrl = gameTitleDiv.GetAttribute("href"); 
                    Game game = new Game(gameTitle, "Nedgame", gamePlatform, gamePrice, gameUrl);
                    Games.Add(game);
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
            }
        }

        public void BolScraping()
        {
            driver = new ChromeDriver(ChromeDriverService, ChromeOptions);
            using (driver)
            {
                driver.Navigate().GoToUrl("https://www.bol.com/be/nl/s/?searchtext=" + GameSearchTerm);
                var timeout = 20000; /* Maximum wait time of 10 seconds */
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
                wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
                Thread.Sleep(3000);
                driver.FindElement(By.CssSelector("#js-first-screen-accept-all-button")).Click();
                Thread.Sleep(3000);
                driver.FindElement(By.CssSelector(".ui-btn.ui-btn--primary.u-disable-mouse.js-country-language-btn")).Click();
                Thread.Sleep(3000);

                for (int i = 0; i < 15; i++)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0,500)");
                    Thread.Sleep(2000);
                }
                Thread.Sleep(3000);
                var gameElements = driver.FindElements(By.CssSelector("li.product-item--row.js_item_root"));
                int amount_of_games = gameElements.Count();
                for (int i = 0; i < amount_of_games; i++)
                {
                    var gameElement = gameElements.ElementAt(i);
                    var gameTitleElement = gameElement.FindElement(By.CssSelector("a.product-title.px_list_page_product_click.list_page_product_tracking_target"));
                    var gameTitle = gameTitleElement.GetAttribute("innerText");
                    var gamePlatformDiv = gameElement.FindElement(By.CssSelector("ul.product-small-specs"));
                    var gamePlatform = gamePlatformDiv.FindElement(By.XPath("li[2]/span")).GetAttribute("innerText");
                    var gamePriceString = gameElement.FindElement(By.CssSelector("span.promo-price")).GetAttribute("innerText");
                    gamePriceString = gamePriceString.Replace(",", ".");
                    gamePriceString = gamePriceString.Replace("-", "");
                    gamePriceString = gamePriceString.Replace("\r\n", ".");
                    gamePriceString = gamePriceString.Trim();
                    double gamePrice = double.Parse(gamePriceString);
                    var gameUrl = gameTitleElement.GetAttribute("href");
                    Game game = new Game(gameTitle, "Bol", gamePlatform, gamePrice, gameUrl);
                    Games.Add(game);
                }
            }
        }

        public void SortGamesByPrice(string param)
        {
            if (param == "A")
            {
                Games = new ObservableCollection<Game>(Games.OrderBy(o => o.Price));

            }
            if (param == "D")
            {
                Games = new ObservableCollection<Game>(Games.OrderByDescending(o => o.Price));

            }
        }


        public void WriteToJson<T>(ObservableCollection<T> observableCollection)
        {
            string json = JsonConvert.SerializeObject(observableCollection);
            System.IO.File.WriteAllText("info.json", json);
        }

        public void CopyToClipboard(string param)
        {

            switch (param)
            {
                case "game":                    
                    Clipboard.SetText(SelectedGame.GameUrl);
                    break;
                case "youtube":
                    Clipboard.SetText(SelectedYoutubeVideo.VideoUrl);
                    break;
                case "job":
                    Clipboard.SetText(SelectedJob.JobUrl);
                    break;               
            }
            
        }
    }
}
