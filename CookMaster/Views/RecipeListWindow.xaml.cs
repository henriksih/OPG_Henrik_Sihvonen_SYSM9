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
    /// Interaction logic for RecipeListWindow.xaml
    /// </summary>
    public partial class RecipeListWindow : Window
    {
        public RecipeListWindow()
        {
            InitializeComponent();
            var userManager = (UserManager)Application.Current.Resources["UserManager"];
            var recipeManager = (RecipeManager)Application.Current.Resources["RecipeManager"];
            var recipeListWindowVM = new RecipeListWindowViewModel(userManager, recipeManager);
            DataContext = recipeListWindowVM;
        }
    }
}
