using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Webscraper_WPF_Maarten_Hens_R0739214.ViewModels;

namespace Webscraper_WPF_Maarten_Hens_R0739214
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainViewModel vm = new MainViewModel();
            Views.MainView view = new Views.MainView();
            view.DataContext = vm;
            view.Show();
        }

    }
}
