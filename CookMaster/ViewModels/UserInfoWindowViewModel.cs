using CookMaster.Managers;
using CookMaster.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    internal class UserInfoWindowViewModel : ViewModelBase
    {
        public UserManager? UserManager { get; }

        private string _username;
        private string _password;
        private string _country;
        private List<string> _countries;
        private string _error;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        public string Country
        {
            get => _country;
            set { _country = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        public List<string> Countries
        {
            get => _countries;
            set
            {
                _countries = value;
                OnPropertyChanged();
            }
        }

    }
}
