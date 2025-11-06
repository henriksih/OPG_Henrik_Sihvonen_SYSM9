using CookMaster.Managers;
using CookMaster.ViewModels;
using System.Windows;

namespace CookMaster.Views
{
    /// <summary>
    /// Interaction logic for ForgotPasswordWindow.xaml
    /// </summary>
    public partial class ForgotPasswordWindow : Window
    {
        public ForgotPasswordWindow()
        {
            InitializeComponent();
            var userManager = (UserManager)Application.Current.Resources["UserManager"];
            var forgotPasswordWindowVM = new ForgotPasswordWindowViewModel(userManager);
            DataContext = forgotPasswordWindowVM;

            // prenumerera på om fönstret är framgångsrikt
            forgotPasswordWindowVM.OnUpdateSuccess += (s, e) =>
            {
                try
                {
                    this.DialogResult = true;
                }
                catch (InvalidOperationException)
                {
                    this.Close();
                }
            };

            // prenumerera på när användaren avbryter
            forgotPasswordWindowVM.OnCancelRequested += (s, e) =>
            {
                try
                {
                    this.DialogResult = false;
                }
                catch (InvalidOperationException)
                {
                    this.Close();
                }
            };
        }
    }
}
