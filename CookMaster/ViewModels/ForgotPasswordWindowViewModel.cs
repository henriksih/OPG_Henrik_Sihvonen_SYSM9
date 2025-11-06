using CookMaster.Managers;
using CookMaster.MVVM;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    internal class ForgotPasswordWindowViewModel : ViewModelBase
    {
        public UserManager? UserManager { get; }

        private string _username;
        private string _password;
        private string _confirmPassword;
        private string _securityQuestion;
        private string _securityAnswerInput;
        private string _error;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
                Error = string.Empty;

                // Load security question for the typed username (if user exists)
                if (!string.IsNullOrWhiteSpace(_username) && UserManager != null)
                {
                    var u = UserManager.FindUser(_username);
                    SecurityQuestion = u?.SecurityQuestion ?? string.Empty;
                }
                else
                {
                    SecurityQuestion = string.Empty;
                }
            }
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

        public string SecurityQuestion
        {
            get => _securityQuestion;
            set { _securityQuestion = value; OnPropertyChanged(); }
        }

        // What the user types as answer
        public string SecurityAnswerInput
        {
            get => _securityAnswerInput;
            set { _securityAnswerInput = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        public string Error
        {
            get => _error;
            set { _error = value; OnPropertyChanged(); }
        }

        public ICommand? UpdateCommand { get; }
        public ICommand? CancelCommand { get; }
        //public Action<object, object> OnCancelRequested { get; internal set; }

        public event EventHandler? OnUpdateSuccess;

        public event EventHandler? OnCancelRequested;


        public ForgotPasswordWindowViewModel(UserManager userManager)
        {
            UserManager = userManager;
            UpdateCommand = new RelayCommand(execute => UpdatePassword(Username));
            CancelCommand = new RelayCommand(_ => OnCancelRequested?.Invoke(this, EventArgs.Empty));

            // initiera för att undvika null-värden
            _username = string.Empty;
            _password = string.Empty;
            _confirmPassword = string.Empty;
            _securityQuestion = string.Empty;
            _securityAnswerInput = string.Empty;
            _error = string.Empty;
        }

        //public bool ValidUser(string Username)
        //{
        //    if (UserManager.FindUser(Username).Username == this.Username && )
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

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

        private void UpdatePassword(string username)
        {
            Error = string.Empty;

            if (string.IsNullOrWhiteSpace(username))
            {
                Error = "Användarnamn måste anges";
                return;
            }

            if (UserManager == null)
            {
                Error = "Försök igen";
                return;
            }

            var user = UserManager.FindUser(username);
            if (user == null)
            {
                Error = "Användaren finns inte";
                return;
            }

            var expected = (user.SecurityAnswer ?? string.Empty).Trim();
            var provided = (SecurityAnswerInput ?? string.Empty).Trim();
            if (!string.Equals(expected, provided, StringComparison.OrdinalIgnoreCase))
            {
                Error = "Svaret på säkerhetsfrågan är fel";
                return;
            }


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

            // Perform the change
            UserManager.ChangePassword(username, Password);

            // Optionally clear sensitive fields
            Password = string.Empty;
            ConfirmPassword = string.Empty;

            OnUpdateSuccess?.Invoke(this, EventArgs.Empty);
        }

        public void Close()
        {
            OnCancelRequested?.Invoke(this, EventArgs.Empty);
        }

    }
}
