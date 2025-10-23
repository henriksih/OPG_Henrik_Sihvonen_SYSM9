using CookMaster.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    public class RegisterWindowViewModel : INotifyPropertyChanged
    {
        public UserManager? UserManager { get; }

        private string _username;
        private string _password;
        private string _country;
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


        public string Error
        {
            get => _error;
            set { _error = value; OnPropertyChanged(); }
        }

        public ICommand? RegisterCommand { get; }

        public bool CanRegister() =>!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);


        //Konstruktor

        public RegisterWindowViewModel(UserManager userManager)
        {
            UserManager = userManager;
            RegisterCommand = new RelayCommand(execute => Register(), canExecute => CanRegister());
        }

        private void Register()
        {

            UserManager.Register(Username, Password, Country);

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }




    }
}
