using CookMaster.Managers;
using CookMaster.ViewModels;
using System.Windows;

namespace CookMaster.Views
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            var userManager = (UserManager)Application.Current.Resources["UserManager"];
            var registerWindowVM = new RegisterWindowViewModel(userManager);
            DataContext = registerWindowVM;

            // prenumerera på om registerfönstret är framgångsrikt
            registerWindowVM.OnRegisterSuccess += (s, e) =>
            {
                // Och om så blir DialogResult sant
                this.DialogResult = true;
            };


        }
    }
}
