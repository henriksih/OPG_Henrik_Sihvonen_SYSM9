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
    /// Interaction logic for RecipeDetailWindow.xaml
    /// </summary>
    public partial class RecipeDetailWindow : Window
    {
        public RecipeDetailWindow()
        {
            InitializeComponent();
            var userManager = (UserManager)Application.Current.Resources["UserManager"];
            var recipeManager = (RecipeManager)Application.Current.Resources["RecipeManager"];
            var recipeDetailWindowVM = new RecipeDetailWindowViewModel(userManager, recipeManager);
            DataContext = recipeDetailWindowVM;

            // prenumerera på om registerfönstret är framgångsrikt
            recipeDetailWindowVM.OnSaveRecipeSuccess += (s, e) =>
            {
                // Och om så blir DialogResult sant
                this.DialogResult = true;
            };

            DataContext = recipeDetailWindowVM;
        }
    }
}
