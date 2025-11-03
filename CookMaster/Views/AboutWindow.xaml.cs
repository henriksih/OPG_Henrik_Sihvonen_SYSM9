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
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            var aboutWindowVM = new AboutWindowViewModel();
            DataContext = aboutWindowVM;


            // prenumerera på om knappen Stäng är tryckt på
            aboutWindowVM.IfClosed += (s, e) =>
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
            //DataContext = aboutWindowVM;
        }
    }
}
