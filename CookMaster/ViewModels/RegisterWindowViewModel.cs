using CookMaster.Managers;
using CookMaster.MVVM;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace CookMaster.ViewModels
{
    public class RegisterWindowViewModel : ViewModelBase
    {
        public UserManager? UserManager { get; }

        private string _username;
        private string _password;
        private string _confirmPassword;
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

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
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

        // Kräv ett usernamn och giltigt passord för att kunna registrera
        public bool CanRegister() =>
            !string.IsNullOrWhiteSpace(Username);
        

            // Om man sätter kraven på lösen här får man inga felmeddelanden
            //&& IsPasswordValid(Password)
            //&& string.Equals(Password, ConfirmPassword, System.StringComparison.Ordinal);

        public event EventHandler? OnRegisterSuccess;

        //Konstruktor
        public RegisterWindowViewModel(UserManager userManager)
        {
            UserManager = userManager;
            RegisterCommand = new RelayCommand(execute => Register(Username), canExecute => CanRegister());
            Countries = new ObservableCollection<string> { "Sverige", "Norge", "Danmark" };
            // initiera för att undvika null-värden
            _password = string.Empty;
            _confirmPassword = string.Empty;
            _username = string.Empty;
            _country = string.Empty;
        }

        private static bool IsPasswordValid(string? pwd)
        {
            //Lösenordsvalidering
            // Inte tomt, minst 8 tecken, minst en siffra och ett specialtecken

            if (string.IsNullOrWhiteSpace(pwd))
                return false;

            
            if (pwd.Length < 8)
                return false;

            if (!Regex.IsMatch(pwd, @"\d"))
                return false;

            if (!Regex.IsMatch(pwd, @"[^A-Za-z0-9]"))
                return false;

            return true;
        }

        private void Register(string name)
        {
            // validera att namn finns och är längre än 3 tecken
            if (string.IsNullOrWhiteSpace(name) || name.Length < 4)
            {
                Error = "Användarnamn måste anges";
                return;
            }

            if (!IsPasswordValid(Password))
            {
                Error = "Lösenordet måste vara minst 8 tecken, innehålla minst en siffra och en specialtecken";
                return;
            }

            if (!string.Equals(Password, ConfirmPassword, System.StringComparison.Ordinal))
            {
                Error = "Lösenorden matchar inte";
                return;
            }

            if (Country == "")
            {
                Error = "Du måste välja ett land";
                return;
            }

            // checka att usernamnet inte redan finns
            if (UserManager != null)
            {
                if (UserManager.FindUser(name) == null)
                {
                    UserManager.Register(Username, Password, Country);

                    // Berätta att registreringen var framgångsrik
                    OnRegisterSuccess?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    Error = "Användarnamnet är redan taget";
                }
            }
            else
            {
                Error = "Försök igen";
            }
        }
    }
}
