using CookMaster.Managers;
using CookMaster.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace CookMaster.Views
{
    /// <summary>
    /// Interaction logic for AddRecipeWindow.xaml
    /// </summary>
    public partial class AddRecipeWindow : Window
    {
        public AddRecipeWindow()
        {
            InitializeComponent();
            var userManager = (UserManager)Application.Current.Resources["UserManager"];
            var recipeManager = (RecipeManager)Application.Current.Resources["RecipeManager"];
            // pass both managers so the VM can default CreatedBy to the logged-in user
            var addRecipeWindowVM = new AddRecipeWindowViewModel(recipeManager, userManager);
            DataContext = addRecipeWindowVM;

            addRecipeWindowVM.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(addRecipeWindowVM.IsAdded) && addRecipeWindowVM.IsAdded)
                {
                    this.DialogResult = true;
                    this.Close();
                }
            };
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Ensure the owner window is shown again if this window is closed
            var recipeListWindow = Application.Current.MainWindow;
            recipeListWindow?.Show();
        }
    }
}
