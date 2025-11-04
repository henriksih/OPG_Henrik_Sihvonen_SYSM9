using CookMaster.Managers;
using CookMaster.Models;
using CookMaster.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    public class RegisterWindowViewModel : ViewModelBase
    {
        public UserManager? UserManager { get; }

        private string _username;
        private string _password;
        private string _country;
        private ObservableCollection<string> _countries;
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

        public ObservableCollection<string> Countries
        {
            get => _countries;
            set
            {
                _countries = value;
                OnPropertyChanged();
            }
        }

        public string Error
        {
            get => _error;
            set { _error = value; OnPropertyChanged(); }
        }

        public ICommand? RegisterCommand { get; }

        public bool CanRegister() =>!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);

        public event EventHandler? OnRegisterSuccess;
        
        //Konstruktor
        public RegisterWindowViewModel(UserManager userManager)
        {
            UserManager = userManager;
            RegisterCommand = new RelayCommand(execute => Register(), canExecute => CanRegister());
            Countries = new ObservableCollection<string> { "Sverige", "Norge", "Danmark" };
        }

        private void Register()
        {
            UserManager.Register(Username, Password, Country);

            // Berätta att registreringen var framgångsrik
            OnRegisterSuccess?.Invoke(this, EventArgs.Empty);
        }
    }
}
