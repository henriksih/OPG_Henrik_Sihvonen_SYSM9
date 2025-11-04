using CookMaster.Managers;
using CookMaster.MVVM;
using CookMaster.Views;
using System.Windows;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public UserManager? UserManager { get; }

        public RecipeManager recipeManager { get; }

        private string _username;
        private string _password;
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

        public string Error
        {
            get => _error;
            set { _error = value; OnPropertyChanged(); }
        }


        public ICommand LoginCommand { get; }
        public ICommand? LogoutCommand { get; }
        public ICommand? RegisterCommand { get; }


        public MainWindowViewModel()
        {
            UserManager = (UserManager)Application.Current.Resources["UserManager"];
            recipeManager = (RecipeManager)Application.Current.Resources["RecipeManager"];

            LoginCommand = new RelayCommand(execute => Login(), canExecute => CanLogin());
            LogoutCommand = new RelayCommand(execute => Logout(), canExecute => UserManager.IsAuthenticated);
            RegisterCommand = new RelayCommand(execute => ShowRegisterWindow());
        }


        public bool CanLogin() =>
            !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);

        public void Login()
        {
            if (UserManager.Login(Username, Password))
            {
                Username = "";
                Password = "";
                Error = string.Empty; // Rensa ev felmeddelande
                ShowRecipeListWindow();
            }
            else
                Error = "Fel användarnamn eller lösenord.";
        }

        // Detta är själva eventet som View (LoginWindow) kan "prenumerera" på
        // När login lyckas, körs alla metoder som är kopplade till detta event.
        public event System.EventHandler OnLoginSuccess;



        public void ShowRecipeListWindow()
        {
            var loggedIn = UserManager.GetLoggedin();
            if (loggedIn == null)
            {
                Error = "No logged-in user found.";
                return;
            }

            // Skapa recipeListWindow med userManager och recipeManager
            var recipeListWindow = new RecipeListWindow(UserManager, recipeManager);

            // Göm loginfönstret om det syns
            var oldMain = Application.Current?.MainWindow;
            if (oldMain != null)
            {
                recipeListWindow.Owner = oldMain; // Gör loginfönstret till förälder
                oldMain.Hide();
            }

            // Gör recipeListWindow till nytt Mainfönster.
            Application.Current.MainWindow = recipeListWindow;
            recipeListWindow.Show();

            // Ingen application.Current.Shutdown
        }

        public void ShowRegisterWindow()
        {
            var registerWindow = new RegisterWindow();
            //Göm MainWindow medan RegisterWindow är öppen
            var currentMain = Application.Current.MainWindow;
            if (currentMain != null)
            {
                registerWindow.Owner = currentMain;
                currentMain.Hide();
            }

            var result = registerWindow.ShowDialog();

            // Om null/false betrakta det som "cancel" — ta tillbaka föregående fönster.
            if (result != true)
            {
                currentMain?.Show();
                return;
            }
            //Om registreringen lyckades, visa MainWindow igen
            currentMain?.Show();
        }



        private void Logout()
        {
            //Logga ut användaren
            UserManager.Logout();

            //Visa MainWindow
            var newMain = new MainWindow();
            Application.Current.MainWindow = newMain;
            newMain.Show();
        }
    }
}
