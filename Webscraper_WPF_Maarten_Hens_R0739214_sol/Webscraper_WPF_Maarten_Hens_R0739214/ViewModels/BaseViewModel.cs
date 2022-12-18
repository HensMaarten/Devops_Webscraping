using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Webscraper_WPF_Maarten_Hens_R0739214.ViewModels
{
    public abstract class BaseViewModel : IDataErrorInfo, INotifyPropertyChanged, ICommand
    {
        #region IDataError
        public abstract string this[string columnName] { get; }

        public string Error
        {
            get
            {
                string errors = "";
                foreach (var item in this.GetType().GetProperties())
                {
                    string error = this[item.Name];
                    if (!string.IsNullOrWhiteSpace(error))
                        errors += error + Environment.NewLine;
                }
                return errors;
            }
        }

        public bool IsValid()
        {
            return string.IsNullOrWhiteSpace(Error);
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region ICommand

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public abstract bool CanExecute(object? parameter);

        public abstract void Execute(object? parameter);
        #endregion
    }
}
