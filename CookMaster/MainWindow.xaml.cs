using CookMaster.Managers;
using CookMaster.ViewModels;
using System.Windows;

namespace CookMaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var userManager = (UserManager)Application.Current.Resources["UserManager"];
            //var recipeManager = (RecipeManager?)Application.Current.Resources["RecipeManager"];
            var mainWindowVM = new MainWindowViewModel();
            DataContext = mainWindowVM;

            mainWindowVM.OnLoginSuccess += (s, e) =>
            {
                mainWindowVM.Password = string.Empty;
                Pwd.Password = "";
                DialogResult = true;
                Hide();
            };
            DataContext = mainWindowVM;
        }

        private void Pwd_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
            {
                vm.Password = Pwd.Password;
            }

        }
    }
}