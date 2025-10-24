using CookMaster.Managers;
using CookMaster.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            var mainWindowVM = new MainWindowViewModel(userManager);
            DataContext = mainWindowVM;

            mainWindowVM.OnLoginSuccess += (s, e) =>
            {
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


        //private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}
    }
}