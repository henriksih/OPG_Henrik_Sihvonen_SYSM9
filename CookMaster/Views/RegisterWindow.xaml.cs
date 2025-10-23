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

            DataContext = registerWindowVM;

        }
    }
}
