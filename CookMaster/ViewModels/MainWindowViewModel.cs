using CookMaster.Managers;
using CookMaster.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CookMaster.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public UserManager? UserManager { get; }

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

        public MainWindowViewModel(UserManager userManager)
        {
            UserManager = userManager;

            //if (!UserManager.IsAuthenticated)
            //{
            //    ShowRecipeListWindow();
            //}
            LoginCommand = new RelayCommand(execute => Login(), canExecute => CanLogin());
            LogoutCommand = new RelayCommand(execute => Logout(), canExecute => UserManager.IsAuthenticated);

        }

       

        public bool CanLogin() =>
            !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);

        public void Login()
        {
            if (UserManager.Login(Username, Password))
            {
                ShowRecipeListWindow();
            }
                //    Om inloggningen lyckas:
                //    → "OnLoginSuccess" är ett event
                //    → Invoke "talar" till alla som lyssnar på det eventet (i det här fallet: LoginWindow)
                //    → (this, EventArgs.Empty) skickar med en referens till vem som skickade eventet + tomma eventdata
                
            else
                Error = "Fel användarnamn eller lösenord.";
        }

        // Detta är själva eventet som View (LoginWindow) kan "prenumerera" på
        // När login lyckas, körs alla metoder som är kopplade till detta event.
        public event System.EventHandler OnLoginSuccess;


        public void ShowRecipeListWindow()
        {
            var recipeListWindow = new RecipeListWindow();
            var result = recipeListWindow.ShowDialog();

            if (result != true)
                Application.Current.Shutdown();
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

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
