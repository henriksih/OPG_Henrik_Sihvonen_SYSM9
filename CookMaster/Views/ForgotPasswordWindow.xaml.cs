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
