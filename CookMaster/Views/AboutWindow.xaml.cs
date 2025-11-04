using CookMaster.ViewModels;
using System.Windows;

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
