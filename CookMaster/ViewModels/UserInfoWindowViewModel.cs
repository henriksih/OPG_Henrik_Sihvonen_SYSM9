using CookMaster.Managers;
using CookMaster.MVVM;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CookMaster.ViewModels
{
    internal class UserInfoWindowViewModel : ViewModelBase
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


        public UserInfoWindowViewModel(UserManager userManager)
        {
            UserManager = userManager;

            // Lägg till länderna som innan
            Countries = new ObservableCollection<string> { "Sverige", "Norge", "Danmark" };

            // Uppdatera fälten med den inloggade usern
            var loggedUser = UserManager?.GetLoggedin();
            if (loggedUser != null)
            {
                Username = loggedUser.Username ?? string.Empty;
                Password = loggedUser.Password ?? string.Empty;

                if (!string.IsNullOrEmpty(loggedUser.Country))
                {
                    var matched = Countries.FirstOrDefault(c => c == loggedUser.Country);
                    Country = matched ?? (Countries.Count > 0 ? Countries[0] : string.Empty);
                }
                else
                {
                    Country = Countries.Count > 0 ? Countries[0] : string.Empty;
                }
            }

            SaveCommand = new RelayCommand(_ => Save(Username));
            CancelCommand = new RelayCommand(execute => Close());
        }
        public ICommand? SaveCommand { get; }
        public ICommand? CancelCommand { get; }

        public event EventHandler? IfClosed;
        public void Close()
        {
            IfClosed?.Invoke(this, EventArgs.Empty);
        }

        public void Save(string name)
        {
            var logged = UserManager?.GetLoggedin();
            if (logged != null)
            {
                if (UserManager?.FindUser(name) == null || UserManager?.FindUser(name) == logged)
                {
                    // spara eventuella ändringar
                    logged.Username = Username;
                    logged.Password = Password;
                    logged.Country = Country;
                    // Stäng fönstret
                    IfClosed?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    Error = "Användarnamnet är redan taget";
                }
            }
            
        }
    }
}
