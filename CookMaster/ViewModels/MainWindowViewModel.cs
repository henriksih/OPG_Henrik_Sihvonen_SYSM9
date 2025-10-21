using CookMaster.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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


        public ICommand? LogoutCommand { get; }

        public MainWindowViewModel(UserManager userManager)
        {
            UserManager = userManager;

            if (!UserManager.IsAuthenticated) Console.WriteLine("RecipeListWindow displayed");//ShowRecipeListWindow(); när den finns

            LogoutCommand = new RelayCommand(execute => Logout(), canExecute => UserManager.IsAuthenticated);

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
