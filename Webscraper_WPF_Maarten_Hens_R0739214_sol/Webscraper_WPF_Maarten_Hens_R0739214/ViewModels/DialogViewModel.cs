using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webscraper_WPF_Maarten_Hens_R0739214.tools;
using Webscraper_WPF_Maarten_Hens_R0739214.Views;

namespace Webscraper_WPF_Maarten_Hens_R0739214.ViewModels
{
    public class DialogViewModel : BaseViewModel, IDialogService
    {

        private DialogView? _dialogView;



        public DialogView? dialogView
        {
            get { return _dialogView; }
            set {
                _dialogView = value;
                NotifyPropertyChanged();
            }
        }

        public string? Title { get; set; }
        public string? Message { get; set; }

        public DialogViewModel(string title, string message)
        {
            this.dialogView = new DialogView();
            Title = title;
            Message = message;
            dialogView.DataContext = this;
        }
        public override string this[string columnName]
        {
            get { return ""; }
        }

        public override bool CanExecute(object? parameter)
        {
            return true;
        }

        public override void Execute(object? parameter)
        {
            if(parameter != null)
            {
                if (parameter.ToString() == "Close")
                {
                    CloseDialog();
                }
            }
        }

        public void ShowDialog()
        {
            if (dialogView != null)
            {
                dialogView.Show();
            }
        }
        public void CloseDialog()
        {
            if (dialogView != null)
            {
                dialogView.Close();
            }
        }

    }
}
