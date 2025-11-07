using CookMaster.Managers;
using CookMaster.ViewModels;
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
            // Skicka båda managers så VM kan sätta CreatedBy till den inloggade usern
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
            // Säkerställ att RecipeListWindow visas om detta stängs
            var recipeListWindow = Application.Current.MainWindow;
            recipeListWindow?.Show();
        }
    }
}
