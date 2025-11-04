using CookMaster.Managers;
using CookMaster.ViewModels;
using System.Windows;

namespace CookMaster.Views
{
    /// <summary>
    /// Interaction logic for UserInfoWindow.xaml
    /// </summary>
    public partial class UserInfoWindow : Window
    {
        public UserInfoWindow()
        {
            InitializeComponent();
            var userManager = (UserManager)Application.Current.Resources["UserManager"];
            var userInfoWindowVM = new UserInfoWindowViewModel(userManager);
            DataContext = userInfoWindowVM;

            // prenumerera på om knappen Stäng är tryckt på
            userInfoWindowVM.IfClosed += (s, e) =>
            {
                try
                {
                    // Och om så blir DialogResult sant
                    this.DialogResult = true;
                }
                catch (InvalidOperationException)
                {
                    this.Close();
                }
            };
        }
    }
}
