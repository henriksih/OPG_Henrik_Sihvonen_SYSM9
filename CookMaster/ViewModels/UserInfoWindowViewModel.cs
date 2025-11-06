using CookMaster.Managers;
using CookMaster.MVVM;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    internal class UserInfoWindowViewModel : ViewModelBase
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
            set
            {
                _username = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
                // ta bort felmeddelande medan man editerar
                Error = string.Empty;
            }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        // New confirm-password property — bind your second password input to this
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
                ConfirmPassword = Password;

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

        private static bool IsPasswordValid(string? pwd)
        {
            // Same rules as RegisterWindow:
            // not empty, at least 8 chars, at least one digit and one special character
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

        public void Save(string name)
        {
            var logged = UserManager?.GetLoggedin();
            if (logged == null)
                return;

            // Spara gamla usernamnet, så recepten kan föras över till det nya
            var oldUsername = logged.Username ?? string.Empty;

            //Validera att det finns ett användarnamn och att det är mer än 3 tecken långt
            if (string.IsNullOrWhiteSpace(name) || name.Length < 4)
            {
                Error = "Användarnamnet måste vara längre än 3 tecken";
                return;
            }

            // tillåt att spara om username inte är använt, eller är samma som innan
            var found = UserManager?.FindUser(name);
            if (found != null && found != logged)
            {
                Error = "Användarnamnet är redan taget";
                return;
            }

            // Validera lösen samma som i RegisterWindow: båda måste finnas, vara giltiga och matcha
            if (!IsPasswordValid(Password))
            {
                Error = "Lösenordet måste vara minst 8 tecken, innehålla minst en siffra och ett specialtecken";
                return;
            }

            if (!string.Equals(Password, ConfirmPassword, StringComparison.Ordinal))
            {
                Error = "Lösenorden matchar inte";
                return;
            }

            try
            {
                // försök använda den globala RecipeManagern om den finns och har recept
                if (Application.Current.Resources["RecipeManager"] is CookMaster.Managers.RecipeManager recipeManager
                    && recipeManager.Recipes != null)
                {
                    foreach (var r in recipeManager.Recipes)
                    {
                        if (string.Equals(r.CreatedBy, oldUsername, StringComparison.OrdinalIgnoreCase))
                        {
                            r.CreatedBy = name;
                        }
                    }

                    // Försäkra att logged userns recept pekar på den delade receptsamlingen
                    logged.MyRecipeList = recipeManager.Recipes;
                }
            }
            catch
            {
                // Informera användaren om det inte gick att föra över recepten men usernamnet blev uppdaterat...
                Error = "Kunde inte uppdatera receptägarskap — men kontot sparades.";
            }

            // spara eventuella ändringar
            logged.Username = name;
            Username = name;
            logged.Password = Password;
            logged.Country = Country;

            // Stäng fönstret
            IfClosed?.Invoke(this, EventArgs.Empty);

        }
    }
}

