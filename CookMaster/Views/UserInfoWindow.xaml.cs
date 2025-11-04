using CookMaster.Managers;
using CookMaster.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
